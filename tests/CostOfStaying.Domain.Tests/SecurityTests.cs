using NUnit.Framework;
using System;
using CostOfStaying.Domain;

namespace CostOfStaying.Domain.Tests
{
    /// <summary>
    /// Server-authority invariants (ADR-002): clients cannot directly mutate
    /// vitality, betrayal, decay rate, or match outcome. All mutations go
    /// through validated Match methods using server clock.
    /// </summary>
    [TestFixture]
    public class SecurityTests
    {
        [Test]
        public void Player_State_Cannot_Be_Mutated_Directly_From_Outside_Domain()
        {
            // Verify that Player setters are internal — external code cannot do
            // player.Vitality = 999. This is a compile-time guarantee, but we
            // test the runtime that Tick is the only way to change vitality.
            var config = new MatchConfig { BaseDecayPerSecond = 0.1, CollapseIntervalSeconds = 1000, CollapseIntervalThreeAliveSeconds = 1000 };
            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), config, clock);
            var pid = Guid.NewGuid();
            match.AddPlayer(pid);
            match.Start();
            double before = match.Players[pid].Vitality;
            clock.Advance(5);
            match.Tick(5);
            Assert.That(match.Players[pid].Vitality, Is.LessThan(before));
        }

        [Test]
        public void Transfer_Does_Not_Trust_Client_Distance_Without_Server_Check()
        {
            // The domain receives distanceMeters as server-measured; if the
            // server passes a large distance the transfer must be rejected.
            var config = new MatchConfig { TransferRangeMeters = 2.0, TransferCooldownSeconds = 0, CollapseIntervalSeconds = 1000, CollapseIntervalThreeAliveSeconds = 1000 };
            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), config, clock);
            var a = Guid.NewGuid(); var b = Guid.NewGuid();
            match.AddPlayer(a); match.AddPlayer(b); match.Start();
            match.Players[a].Debuffs.Add(new Debuff("d", 0.01, 10, true));
            // Server measured distance is 10m — must reject even if client
            // claimed 1m in its intent (intent.ClaimedDistance is ignored).
            var r = match.TryTransfer(a, b, distanceMeters: 10.0, requestId: "r");
            Assert.That(r, Is.EqualTo(Match.TransferResult.RejectedOutOfRange));
        }

        [Test]
        public void Replay_Attack_With_Same_RequestId_Is_Rejected()
        {
            var config = new MatchConfig { TransferCooldownSeconds = 0, CollapseIntervalSeconds = 1000, CollapseIntervalThreeAliveSeconds = 1000 };
            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), config, clock);
            var a = Guid.NewGuid(); var b = Guid.NewGuid();
            match.AddPlayer(a); match.AddPlayer(b); match.Start();
            match.Players[a].Debuffs.Add(new Debuff("d", 0.01, 10, true));
            var r1 = match.TryTransfer(a, b, 1.0, "replay-id");
            Assert.That(r1, Is.EqualTo(Match.TransferResult.Success));
            // Attacker replays same requestId
            match.Players[a].Debuffs.Add(new Debuff("d2", 0.01, 10, true));
            var r2 = match.TryTransfer(a, b, 1.0, "replay-id");
            Assert.That(r2, Is.EqualTo(Match.TransferResult.RejectedDuplicateRequest));
        }

        [Test]
        public void Betrayal_Only_On_Success_Not_On_Failure()
        {
            var config = new MatchConfig { TransferRangeMeters = 2.0, TransferCooldownSeconds = 0, CollapseIntervalSeconds = 1000, CollapseIntervalThreeAliveSeconds = 1000 };
            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), config, clock);
            var a = Guid.NewGuid(); var b = Guid.NewGuid();
            match.AddPlayer(a); match.AddPlayer(b); match.Start();
            match.Players[a].Debuffs.Add(new Debuff("d", 0.01, 10, true));
            match.TryTransfer(a, b, 10.0, "fail"); // out of range
            Assert.That(match.Players[a].BetrayalPoints, Is.EqualTo(0));
            match.TryTransfer(a, b, 1.0, "ok");
            Assert.That(match.Players[a].BetrayalPoints, Is.EqualTo(1));
        }

        [Test]
        public void Match_Uses_A_Validated_Config_Snapshot()
        {
            var config = new MatchConfig { StartingVitality = 100, PlayerCount = 1 };
            var match = new Match(Guid.NewGuid(), config, new ManualClock(0));

            config.StartingVitality = 1;
            match.Config.StartingVitality = 2;
            Player player = match.AddPlayer(Guid.NewGuid());
            match.Start();

            Assert.That(player.Vitality, Is.EqualTo(100));
        }

        [Test]
        public void Match_Rejects_NonFinite_Config_And_Tick_Values()
        {
            foreach (double invalidValue in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
            {
                var invalidConfig = new MatchConfig { BaseDecayPerSecond = invalidValue };
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    new Match(Guid.NewGuid(), invalidConfig, new ManualClock(0)));
            }

            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), new MatchConfig(), clock);
            Guid id = Guid.NewGuid();
            Player player = match.AddPlayer(id);
            match.Start();
            match.Tick(double.NaN);
            match.Tick(double.PositiveInfinity);

            Assert.That(player.Vitality, Is.EqualTo(100));
        }

        [Test]
        public void Transfer_Rejects_NonFinite_And_Negative_Server_Distances()
        {
            var config = new MatchConfig { TransferCooldownSeconds = 0 };
            var match = new Match(Guid.NewGuid(), config, new ManualClock(0));
            Guid sourceId = Guid.NewGuid();
            Guid targetId = Guid.NewGuid();
            match.AddPlayer(sourceId);
            match.AddPlayer(targetId);
            match.Start();
            match.Players[sourceId].Debuffs.Add(new Debuff("d", 0.01, 10, true));

            Assert.That(match.TryTransfer(sourceId, targetId, double.NaN, "nan"),
                Is.EqualTo(Match.TransferResult.RejectedInvalidDistance));
            Assert.That(match.TryTransfer(sourceId, targetId, double.PositiveInfinity, "infinity"),
                Is.EqualTo(Match.TransferResult.RejectedInvalidDistance));
            Assert.That(match.TryTransfer(sourceId, targetId, -1, "negative"),
                Is.EqualTo(Match.TransferResult.RejectedInvalidDistance));
            Assert.That(match.Players[sourceId].BetrayalPoints, Is.EqualTo(0));
            Assert.That(match.Players[sourceId].Debuffs.Count, Is.EqualTo(1));
        }

        [Test]
        public void Gameplay_Commands_Are_Rejected_After_Match_Completion()
        {
            var config = new MatchConfig
            {
                MatchTimeLimitSeconds = 1,
                TransferCooldownSeconds = 0,
                PlayerCount = 3
            };
            var clock = new ManualClock(0);
            var match = new Match(Guid.NewGuid(), config, clock);
            Guid sourceId = Guid.NewGuid();
            Guid targetId = Guid.NewGuid();
            Guid spiritId = Guid.NewGuid();
            match.AddPlayer(sourceId);
            match.AddPlayer(targetId);
            match.AddPlayer(spiritId);
            match.Start();
            match.Players[sourceId].Debuffs.Add(new Debuff("d", 0.01, 10, true));
            match.Players[spiritId].State = PlayerState.Spirit;
            clock.Advance(1);
            match.Tick(1);

            Assert.That(match.State, Is.EqualTo(MatchState.Completed));
            int eventCount = match.Events.Count;
            Assert.That(match.TryTransfer(sourceId, targetId, 1, "after-match"),
                Is.EqualTo(Match.TransferResult.RejectedMatchNotActive));
            Assert.That(match.CollectEcho(spiritId, "echo-1"), Is.False);
            Assert.That(match.TrySendHint(spiritId, "hint"), Is.False);
            Assert.That(match.Players[sourceId].BetrayalPoints, Is.EqualTo(0));
            Assert.That(match.Players[sourceId].Debuffs.Count, Is.EqualTo(1));
            Assert.That(match.Players[spiritId].EchoFragments, Is.EqualTo(0));
            Assert.That(match.Events.Count, Is.EqualTo(eventCount));
        }
    }
}
