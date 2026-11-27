namespace CostOfStaying.Core
{
    using System;
    using CostOfStaying.Core.Contracts;
    using CostOfStaying.Core.Domain;
    using CostOfStaying.Core.Services;

    /// <summary>
    /// Composition root for exactly one server-owned match scope.
    /// The required MatchId binds the owned betrayal ledger and every match-scoped
    /// aggregate/service created by this composition. Session/host registries remain
    /// separately managed resources. All mutation is single-writer/server-serialized.
    /// </summary>
    public sealed class CoreComposition
    {
        private readonly IBetrayalLedger _betrayalLedger;

        private CoreComposition(MatchId matchId)
        {
            if (!matchId.IsValid)
                throw new ArgumentException("A valid MatchId is required.", nameof(matchId));

            MatchId = matchId;
            _betrayalLedger = new BetrayalLedger();
        }

        public MatchId MatchId { get; }

        public IBetrayalLedger BetrayalLedger => _betrayalLedger;

        public static CoreComposition Create(MatchId matchId) => new CoreComposition(matchId);

        public IVitalityAccount CreateVitalityAccount(ActorId playerId, uint maximum = VitalityAccount.DefaultMaximum)
        {
            return new VitalityAccount(MatchId, playerId, maximum);
        }

        public IMatchLifecycle CreateMatchLifecycle()
        {
            return new MatchLifecycle(MatchId);
        }

        public ISessionLifecycle CreateSessionLifecycle(SessionId sessionId)
        {
            return new SessionLifecycle(sessionId);
        }

        public SessionRegistry CreateSessionRegistry(int capacity)
        {
            return new SessionRegistry(capacity);
        }

        public IConnectionSessionRegistry CreateConnectionSessionRegistry(ISessionRegistry sessions, int capacity)
        {
            return new ConnectionSessionRegistry(sessions, capacity);
        }

        public IMatchPlayerRegistry CreateMatchPlayerRegistry(int capacity)
        {
            return new MatchPlayerRegistry(MatchId, capacity);
        }

        public IMatchRosterSnapshotBuilder CreateMatchRosterSnapshotBuilder(
            IMatchLifecycle match,
            IMatchPlayerRegistry players)
        {
            EnsureMatchScope(match, nameof(match));
            EnsureMatchScope(players, nameof(players));
            return new MatchRosterSnapshotBuilder(match, players);
        }

        public ICommandAdmissionService<TCommand> CreateCommandAdmissionService<TCommand>(
            ISessionRegistry sessions,
            CommandAdmissionQueue<TCommand> queue)
        {
            return new CommandAdmissionService<TCommand>(sessions, queue);
        }

        public IMatchCommandAdmissionService<TCommand> CreateMatchCommandAdmissionService<TCommand>(
            ISessionRegistry sessions,
            IMatchPlayerRegistry players,
            CommandAdmissionQueue<TCommand> queue)
        {
            EnsureMatchScope(players, nameof(players));
            ICommandAdmissionService<TCommand> inner = new CommandAdmissionService<TCommand>(sessions, queue);
            return new MatchCommandAdmissionService<TCommand>(sessions, players, inner);
        }

        public IMatchCommandAuthorizationService<TCommand> CreateMatchCommandAuthorizationService<TCommand>(
            ISessionRegistry sessions,
            IMatchLifecycle match,
            IMatchPlayerRegistry players,
            CommandAdmissionQueue<TCommand> queue,
            IMatchCommandAuthorizationPolicy<TCommand> policy)
        {
            EnsureMatchScope(match, nameof(match));
            EnsureMatchScope(players, nameof(players));

            IMatchCommandAdmissionService<TCommand> admission =
                CreateMatchCommandAdmissionService(sessions, players, queue);
            return new MatchCommandAuthorizationService<TCommand>(
                sessions,
                match,
                players,
                admission,
                policy);
        }

        public IMatchCommandPayloadValidationService<TCommand> CreateMatchCommandPayloadValidationService<TCommand>(
            IMatchCommandAuthorizationService<TCommand> authorization,
            ICommandPayloadValidator<TCommand> validator)
        {
            if (authorization is null)
                throw new ArgumentNullException(nameof(authorization));
            if (authorization.MatchId != MatchId)
                throw new ArgumentException("Authorization service belongs to a different MatchId.", nameof(authorization));

            return new MatchCommandPayloadValidationService<TCommand>(authorization, validator);
        }

        public ISessionCleanupService<TCommand> CreateSessionCleanupService<TCommand>(
            SessionRegistry sessions,
            CommandAdmissionQueue<TCommand> queue,
            IConnectionSessionRegistry connections)
        {
            if (!(connections is ConnectionSessionRegistry connectionRegistry) ||
                !connectionRegistry.IsBackedBy(sessions))
            {
                throw new ArgumentException(
                    "Connection registry must use the same SessionRegistry instance as cleanup.",
                    nameof(connections));
            }

            ISessionCleanupService<TCommand> inner = new SessionCleanupService<TCommand>(sessions, queue);
            return new ConnectionAwareSessionCleanupService<TCommand>(inner, connections);
        }

        public CommandAdmissionQueue<TCommand> CreateCommandAdmissionQueue<TCommand>(
            ulong maximumSequenceJump,
            int maximumTrackedSessions,
            int capacity)
        {
            return new CommandAdmissionQueue<TCommand>(maximumSequenceJump, maximumTrackedSessions, capacity);
        }

        private void EnsureMatchScope(IMatchLifecycle match, string parameterName)
        {
            if (match is null)
                throw new ArgumentNullException(parameterName);
            if (match.MatchId != MatchId)
                throw new ArgumentException("Match lifecycle belongs to a different MatchId.", parameterName);
        }

        private void EnsureMatchScope(IMatchPlayerRegistry players, string parameterName)
        {
            if (players is null)
                throw new ArgumentNullException(parameterName);
            if (players.MatchId != MatchId)
                throw new ArgumentException("Match player registry belongs to a different MatchId.", parameterName);
        }
    }
}
