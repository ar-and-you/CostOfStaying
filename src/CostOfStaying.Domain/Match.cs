using System;
using System.Collections.Generic;
using System.Linq;

namespace CostOfStaying.Domain
{
    /// <summary>
    /// Domain match aggregate. It validates gameplay state transitions using
    /// supplied inputs and an injected clock. This assembly has no transport,
    /// authentication, or server runtime; a host adapter must enforce those boundaries.
    /// This class has no Unity or FishNet dependencies.
    /// </summary>
    public sealed class Match
    {
        public Guid Id { get; }
        private readonly MatchConfig _config;
        public MatchConfig Config => _config.CreateValidatedSnapshot();
        public IMatchClock Clock { get; }
        public MatchState State { get; private set; } = MatchState.Created;
        public MatchOutcome Outcome { get; private set; } = MatchOutcome.None;
        public Guid? WinnerId { get; private set; }

        /// <summary>Players by id. Includes Alive and Spirit participants.</summary>
        public Dictionary<Guid, Player> Players { get; } = new Dictionary<Guid, Player>();

        /// <summary>Ordered domain events emitted since match creation.</summary>
        public List<DomainEvent> Events { get; } = new List<DomainEvent>();

        /// <summary>Idempotency store for transfer request ids (SUFFERING_TRANSFER_SYSTEM).</summary>
        public HashSet<string> ProcessedRequestIds { get; } = new HashSet<string>();

        // --- Vote state (RULE-03) ---
        public double? VoteOpenedAtSeconds { get; private set; }
        public double? VoteDeadlineSeconds { get; private set; }
        public Dictionary<Guid, VoteChoice> Votes { get; } = new Dictionary<Guid, VoteChoice>();
        public bool VoteHasBeenOpened { get; private set; }

        // --- Time limit reference ---
        public double? StartedAtSeconds { get; private set; }

        // Pending collapse reschedule flag after state changes
        private bool _collapseRescheduleNeeded;
        private bool IsGameplayActive => State == MatchState.Running || State == MatchState.VoteOpen;

        public Match(Guid id, MatchConfig config, IMatchClock clock)
        {
            Id = id;
            _config = (config ?? throw new ArgumentNullException(nameof(config))).CreateValidatedSnapshot();
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        // -----------------------------------------------------------------
        // Lifecycle
        // -----------------------------------------------------------------

        public Player AddPlayer(Guid playerId)
        {
            if (State != MatchState.Created && State != MatchState.Waiting)
                throw new InvalidOperationException($"Cannot add player in state {State}.");
            if (Players.ContainsKey(playerId))
                throw new InvalidOperationException($"Player {playerId} already in match.");
            if (Players.Count >= _config.PlayerCount)
                throw new InvalidOperationException($"Match is full ({_config.PlayerCount}).");

            var player = new Player(playerId, _config);
            Players[playerId] = player;

            if (State == MatchState.Created)
                State = MatchState.Waiting;

            return player;
        }

        public void Start()
        {
            if (State != MatchState.Waiting && State != MatchState.Created)
                throw new InvalidOperationException($"Cannot start from {State}.");
            if (Players.Count == 0)
                throw new InvalidOperationException("Cannot start with no players.");

            State = MatchState.Running;
            double startTime = Clock.NowSeconds;
            StartedAtSeconds = startTime;
            foreach (Player player in Players.Values)
                player.State = PlayerState.Alive;

            // Establish the full roster before deriving one shared interval.
            double initialInterval = CurrentCollapseIntervalSeconds();
            foreach (Player player in Players.Values)
            {
                player.NextCollapseAtSeconds = startTime + initialInterval;
                player.LastCollapseIntervalSeconds = initialInterval;
            }

            Emit(DomainEventKind.MatchStarted, null, null, $"players={Players.Count}");
        }

        // -----------------------------------------------------------------
        // Tick — simulation advance (vitality drain, debuff expiry, collapse, vote, time limit)
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the simulation by the caller-provided deltaSeconds. A production
        /// host should derive this from a monotonic clock; this aggregate does not
        /// read wall time, authenticate the caller, or verify the delta's source.
        /// </summary>
        public void Tick(double deltaSeconds)
        {
            if (State == MatchState.Completed || State == MatchState.Ending)
                return;
            if (State != MatchState.Running && State != MatchState.VoteOpen)
                return;
            if (deltaSeconds <= 0 || double.IsNaN(deltaSeconds) || double.IsInfinity(deltaSeconds))
                return;

            double now = Clock.NowSeconds;

            // 1) Vitality drain and debuff expiry for alive players.
            // Split long ticks at debuff-expiry boundaries so a debuff only
            // contributes decay for the time during which it is active.
            foreach (Player player in Players.Values.Where(p => p.State == PlayerState.Alive))
            {
                AdvanceVitalityAndDebuffs(player, deltaSeconds);
            }

            // 2) Death transitions (RULE-01: death only at zero, one transition)
            var newlyDead = new List<Player>();
            foreach (Player player in Players.Values.Where(p => p.State == PlayerState.Alive && p.Vitality <= 0))
            {
                newlyDead.Add(player);
            }
            foreach (Player player in newlyDead)
            {
                TransitionToSpirit(player, now);
            }

            if (newlyDead.Count > 0)
            {
                _collapseRescheduleNeeded = true;
            }

            // 3) Collapse events per alive player (RULE-01)
            if (State == MatchState.Running || State == MatchState.VoteOpen)
            {
                // Snapshot to avoid modification during iteration
                var aliveSnapshot = Players.Values.Where(p => p.State == PlayerState.Alive).ToList();
                foreach (Player player in aliveSnapshot)
                {
                    if (now >= player.NextCollapseAtSeconds)
                    {
                        ApplyCollapse(player, now);
                    }
                }
            }

            // 4) Reschedule collapses if alive count changed
            if (_collapseRescheduleNeeded)
            {
                RescheduleCollapses(now);
                _collapseRescheduleNeeded = false;
            }

            // 5) Vote timeout check (RULE-03)
            if (State == MatchState.VoteOpen && VoteDeadlineSeconds.HasValue && now >= VoteDeadlineSeconds.Value)
            {
                CloseVote(now);
                // CloseVote may have ended the match; if so, stop further checks
                if (State == MatchState.Completed || State == MatchState.Ending)
                    return;
            }

            // 6) Maybe open vote (RULE-03: when alive becomes three, once per match)
            if (State == MatchState.Running && !VoteHasBeenOpened && AliveCount() == 3)
            {
                OpenVote(now);
            }

            // 7) End conditions (RULE-05)
            EvaluateEndConditions(now);
        }

        // -----------------------------------------------------------------
        // Collapse (RULE-01 + COLLAPSE_SYSTEM reschedule policy)
        // -----------------------------------------------------------------

        private void ApplyCollapse(Player player, double now)
        {
            player.BaseDecayPerSecond += _config.CollapseDecayIncreasePerSecond;

            if (player.HasDebuffCapacity(_config))
            {
                var debuff = new Debuff(
                    id: $"collapse-{Guid.NewGuid():N}",
                    decayBonusPerSecond: _config.DebuffDecayBonusPerSecond,
                    durationSeconds: _config.DebuffDurationSeconds,
                    transferable: true);
                player.Debuffs.Add(debuff);
            }

            double interval = CurrentCollapseIntervalSeconds();
            player.NextCollapseAtSeconds = now + interval;
            player.LastCollapseIntervalSeconds = interval;

            Emit(DomainEventKind.CollapseApplied, player.Id, null, $"interval={interval:F1}s decay={player.BaseDecayPerSecond:F3}");
        }

        private double CurrentCollapseIntervalSeconds()
        {
            return AliveCount() == 3
                ? _config.CollapseIntervalThreeAliveSeconds
                : _config.CollapseIntervalSeconds;
        }

        private void RescheduleCollapses(double now)
        {
            double newInterval = CurrentCollapseIntervalSeconds();
            foreach (Player player in Players.Values.Where(p => p.State == PlayerState.Alive))
            {
                double oldInterval = player.LastCollapseIntervalSeconds;
                if (oldInterval <= 0)
                {
                    player.NextCollapseAtSeconds = now + newInterval;
                    player.LastCollapseIntervalSeconds = newInterval;
                    continue;
                }

                double remaining = player.NextCollapseAtSeconds - now;
                double ratio = Math.Max(0.0, Math.Min(1.0, remaining / oldInterval));
                player.NextCollapseAtSeconds = now + ratio * newInterval;
                player.LastCollapseIntervalSeconds = newInterval;
            }
        }

        // -----------------------------------------------------------------
        // Death -> Spirit
        // -----------------------------------------------------------------

        private void TransitionToSpirit(Player player, double now)
        {
            player.State = PlayerState.Spirit;
            // Spirit starts with zero vitality, no further decay
            player.Vitality = 0;
            Emit(DomainEventKind.PlayerDied, player.Id, null, "Alive->Spirit");
        }

        // -----------------------------------------------------------------
        // Suffering Transfer (RULE-02)
        // -----------------------------------------------------------------

        public enum TransferResult
        {
            Success,
            RejectedDifferentMatch, // not applicable in single-match aggregate, kept for API completeness
            RejectedSelfTarget,
            RejectedNotAlive,
            RejectedNoDebuff,
            RejectedOutOfRange,
            RejectedCooldown,
            RejectedTargetFull,
            RejectedDuplicateRequest,
            RejectedMatchNotActive,
            RejectedInvalidDistance,
            RejectedInvalidRequestId
        }

        /// <summary>
        /// Attempt a suffering transfer. All conditions are checked atomically;
        /// on failure no gameplay state changes; rejection events may be recorded (RULE-02).
        /// distanceMeters is supplied by the caller. A trusted server integration
        /// must measure it; this aggregate validates the supplied value but does not
        /// measure player positions or authenticate the caller.
        /// </summary>
        public TransferResult TryTransfer(Guid sourceId, Guid targetId, double distanceMeters, string requestId)
        {
            double now = Clock.NowSeconds;

            if (!IsGameplayActive)
                return TransferResult.RejectedMatchNotActive;
            if (double.IsNaN(distanceMeters) || double.IsInfinity(distanceMeters) || distanceMeters < 0)
                return TransferResult.RejectedInvalidDistance;
            if (string.IsNullOrWhiteSpace(requestId))
                return TransferResult.RejectedInvalidRequestId;

            // A duplicate request never repeats gameplay mutation; the rejection is recorded.
            if (!string.IsNullOrEmpty(requestId) && ProcessedRequestIds.Contains(requestId))
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, $"duplicate requestId={requestId}");
                return TransferResult.RejectedDuplicateRequest;
            }

            if (sourceId == targetId)
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "self-target");
                return TransferResult.RejectedSelfTarget;
            }

            if (!Players.TryGetValue(sourceId, out Player? source) ||
                !Players.TryGetValue(targetId, out Player? target))
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "unknown player");
                return TransferResult.RejectedNotAlive;
            }

            if (source.State != PlayerState.Alive || target.State != PlayerState.Alive)
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "not alive");
                return TransferResult.RejectedNotAlive;
            }

            Debuff? candidate = source.FindLastTransferableDebuff();
            if (candidate == null)
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "no transferable debuff");
                return TransferResult.RejectedNoDebuff;
            }

            if (distanceMeters > _config.TransferRangeMeters + 1e-9)
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, $"out of range {distanceMeters:F2}m > {_config.TransferRangeMeters:F2}m");
                return TransferResult.RejectedOutOfRange;
            }

            if (now - source.LastTransferAtSeconds < _config.TransferCooldownSeconds - 1e-9)
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "cooldown");
                return TransferResult.RejectedCooldown;
            }

            if (!target.HasDebuffCapacity(_config))
            {
                Emit(DomainEventKind.TransferRejected, sourceId, targetId, "target full");
                return TransferResult.RejectedTargetFull;
            }

            // ---- Atomic move ----
            source.Debuffs.Remove(candidate);
            // Preserve remaining duration and bonus
            var moved = new Debuff(candidate.Id, candidate.DecayBonusPerSecond, candidate.RemainingSeconds, candidate.Transferable);
            target.Debuffs.Add(moved);

            source.BetrayalPoints += 1;
            source.LastTransferAtSeconds = now;

            if (!string.IsNullOrEmpty(requestId))
                ProcessedRequestIds.Add(requestId);

            Emit(DomainEventKind.DebuffTransferred, sourceId, targetId, $"debuff={candidate.Id}");
            Emit(DomainEventKind.BetrayalIncreased, sourceId, null, $"betrayal={source.BetrayalPoints}");

            return TransferResult.Success;
        }

        // -----------------------------------------------------------------
        // Vote (RULE-03)
        // -----------------------------------------------------------------

        private void OpenVote(double now)
        {
            VoteHasBeenOpened = true;
            State = MatchState.VoteOpen;
            VoteOpenedAtSeconds = now;
            VoteDeadlineSeconds = now + _config.VoteDurationSeconds;
            Votes.Clear();
            // Voters are the currently alive players; store Abstain as default
            foreach (Player player in Players.Values.Where(p => p.State == PlayerState.Alive))
            {
                Votes[player.Id] = VoteChoice.Abstain;
            }
            Emit(DomainEventKind.VoteOpened, null, null, $"deadline={VoteDeadlineSeconds.Value:F1}s voters={Votes.Count}");
        }

        public bool CastVote(Guid playerId, VoteChoice choice)
        {
            if (State != MatchState.VoteOpen)
                return false;
            if (!Enum.IsDefined(typeof(VoteChoice), choice))
                return false;
            if (!Votes.ContainsKey(playerId))
                return false;
            if (Players.TryGetValue(playerId, out Player? player) && player.State != PlayerState.Alive)
                return false;

            double now = Clock.NowSeconds;
            if (VoteDeadlineSeconds.HasValue && now >= VoteDeadlineSeconds.Value)
            {
                CloseVote(now);
                return false;
            }

            Votes[playerId] = choice;
            Emit(DomainEventKind.VoteCast, playerId, null, choice.ToString());
            if (Votes.Count > 0 && Votes.Values.All(v => v == VoteChoice.Yes))
                CloseVote(now);
            return true;
        }

        private void CloseVote(double now)
        {
            // The electorate is the snapshot captured by OpenVote.
            // Abstain and timeout prevent unanimity; Domain has no disconnect event integration.
            bool unanimous = Votes.Count > 0 && Votes.Values.All(v => v == VoteChoice.Yes);

            Emit(DomainEventKind.VoteClosed, null, null, unanimous ? "unanimous Yes" : "not unanimous");

            // Only unanimous Yes across the original voter snapshot succeeds.
            // No transport disconnect/reconnect policy is implemented here.
            if (unanimous)
            {
                // Collective Surrender ends match immediately
                Outcome = MatchOutcome.CollectiveSurrender;
                WinnerId = null;
                State = MatchState.Ending;
                Emit(DomainEventKind.MatchEnded, null, null, "CollectiveSurrender");
                State = MatchState.Completed;
                return;
            }

            // Not unanimous: return to Running, keep collapse interval for 3 alive
            State = MatchState.Running;
            VoteOpenedAtSeconds = null;
            VoteDeadlineSeconds = null;
            Votes.Clear();

            // Reschedule collapses to the 3-alive interval (15s initial value)
            RescheduleCollapses(now);
        }

        // -----------------------------------------------------------------
        // Spirit (RULE-04)
        // -----------------------------------------------------------------

        public bool CollectEcho(Guid playerId, string echoId)
        {
            if (!IsGameplayActive || string.IsNullOrWhiteSpace(echoId))
                return false;
            if (!Players.TryGetValue(playerId, out Player? player))
                return false;
            if (player.State != PlayerState.Spirit)
                return false;
            if (player.CollectedEchoIds.Contains(echoId))
                return false;

            player.CollectedEchoIds.Add(echoId);
            player.EchoFragments += 1;
            player.WitnessScore += 1; // 1:1 for MVP; balance may change after playtest
            Emit(DomainEventKind.EchoCollected, playerId, null, echoId);
            return true;
        }

        public bool TrySendHint(Guid spiritId, string hintText)
        {
            if (!IsGameplayActive || string.IsNullOrWhiteSpace(hintText))
                return false;
            if (!Players.TryGetValue(spiritId, out Player? spirit))
                return false;
            if (spirit.State != PlayerState.Spirit)
                return false;

            double now = Clock.NowSeconds;
            if (now - spirit.LastHintAtSeconds < _config.HintCooldownSeconds - 1e-9)
                return false;

            spirit.LastHintAtSeconds = now;
            Emit(DomainEventKind.HintSent, spiritId, null, hintText);
            return true;
        }

        // -----------------------------------------------------------------
        // End conditions (RULE-05)
        // -----------------------------------------------------------------

        private void EvaluateEndConditions(double now)
        {
            if (State == MatchState.Completed || State == MatchState.Ending)
                return;

            int alive = AliveCount();
            bool hasDeath = Players.Values.Any(p => p.State == PlayerState.Spirit || p.State == PlayerState.Dead);

            // Mutual Collapse: no alive left — only if someone actually died
            if (alive == 0 && hasDeath)
            {
                Outcome = MatchOutcome.MutualCollapse;
                WinnerId = null;
                State = MatchState.Ending;
                Emit(DomainEventKind.MatchEnded, null, null, "MutualCollapse");
                State = MatchState.Completed;
                return;
            }

            // Last Survivor takes precedence when only one player remains alive.
            if (alive == 1 && hasDeath)
            {
                Player winner = Players.Values.First(p => p.State == PlayerState.Alive);
                Outcome = MatchOutcome.LastSurvivor;
                WinnerId = winner.Id;
                State = MatchState.Ending;
                Emit(DomainEventKind.MatchEnded, winner.Id, null, "LastSurvivor");
                State = MatchState.Completed;
                return;
            }

            // Rank any living roster at the time limit. A sole player after a
            // death has already been resolved as LastSurvivor above.
            if (alive > 0 && StartedAtSeconds.HasValue && now - StartedAtSeconds.Value >= _config.MatchTimeLimitSeconds - 1e-9)
            {
                var alivePlayers = Players.Values.Where(p => p.State == PlayerState.Alive).ToList();
                double maxVitality = alivePlayers.Max(p => p.Vitality);
                var top = alivePlayers.Where(p => Math.Abs(p.Vitality - maxVitality) < 1e-9).ToList();

                if (top.Count > 1)
                {
                    Outcome = MatchOutcome.Draw;
                    WinnerId = null;
                    Emit(DomainEventKind.MatchEnded, null, null, $"Draw at {maxVitality:F2}");
                }
                else
                {
                    Outcome = MatchOutcome.TimeLimit;
                    WinnerId = top[0].Id;
                    Emit(DomainEventKind.MatchEnded, top[0].Id, null, $"TimeLimit winner vitality={maxVitality:F2}");
                }

                State = MatchState.Ending;
                State = MatchState.Completed;
                return;
            }

        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        public int AliveCount()
        {
            int count = 0;
            foreach (Player player in Players.Values)
            {
                if (player.State == PlayerState.Alive)
                    count++;
            }
            return count;
        }

        private static void AdvanceVitalityAndDebuffs(Player player, double deltaSeconds)
        {
            double remainingSeconds = deltaSeconds;

            while (remainingSeconds > 0)
            {
                // Remove effects that are already expired before calculating this segment.
                for (int i = player.Debuffs.Count - 1; i >= 0; i--)
                {
                    if (player.Debuffs[i].RemainingSeconds <= 0)
                        player.Debuffs.RemoveAt(i);
                }

                // The next segment ends at the earliest debuff expiry, or at the end of the tick.
                double segmentSeconds = remainingSeconds;
                foreach (Debuff debuff in player.Debuffs)
                    segmentSeconds = Math.Min(segmentSeconds, debuff.RemainingSeconds);

                // Debuff construction validates positive finite durations. This guard
                // prevents a malformed in-memory value from causing a non-progressing loop.
                if (segmentSeconds <= 0 || double.IsNaN(segmentSeconds) || double.IsInfinity(segmentSeconds))
                    break;

                double decay = player.TotalDecayPerSecond;
                player.Vitality = Math.Max(0.0, player.Vitality - decay * segmentSeconds);

                foreach (Debuff debuff in player.Debuffs)
                    debuff.RemainingSeconds -= segmentSeconds;

                remainingSeconds = Math.Max(0.0, remainingSeconds - segmentSeconds);
            }

            // Normalize boundary values and remove all effects expired at the tick endpoint.
            for (int i = player.Debuffs.Count - 1; i >= 0; i--)
            {
                if (player.Debuffs[i].RemainingSeconds <= 0)
                    player.Debuffs.RemoveAt(i);
            }
        }

        private void Emit(DomainEventKind kind, Guid? playerId, Guid? targetId, string? detail)
        {
            Events.Add(new DomainEvent(kind, Clock.NowSeconds, playerId, targetId, detail));
        }
    }
}
