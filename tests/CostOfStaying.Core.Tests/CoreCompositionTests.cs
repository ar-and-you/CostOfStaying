namespace CostOfStaying.Core.Tests
{
    using CostOfStaying;
    using CostOfStaying.Core.Domain;
    using CostOfStaying.Core.Contracts;
    using CostOfStaying.Core.Services;
    using NUnit.Framework;

    [TestFixture]
    public sealed class CoreCompositionTests
    {
        private sealed class AllowAllTestPolicy : IMatchCommandAuthorizationPolicy<string>
        {
            public Result Authorize(MatchCommandAuthorizationContext<string> context)
            {
                return Result.Success();
            }
        }

        private sealed class AllowAllTestValidator : ICommandPayloadValidator<string>
        {
            public Result Validate(string command) => Result.Success();
        }

        [Test]
        public void CoreServices_ExposeNoPublicConstructors()
        {
            System.Type[] serviceTypes =
            {
                typeof(VitalityAccount),
                typeof(BetrayalLedger),
                typeof(MatchLifecycle),
                typeof(SessionLifecycle),
                typeof(SessionRegistry),
                typeof(ConnectionSessionRegistry),
                typeof(MatchPlayerRegistry),
                typeof(CommandSequenceTracker),
                typeof(CommandAdmissionQueue<string>),
                typeof(CommandAdmissionService<string>),
                typeof(MatchCommandAdmissionService<string>),
                typeof(MatchCommandAuthorizationService<string>),
                typeof(MatchCommandPayloadValidationService<string>),
                typeof(SessionCleanupService<string>),
                typeof(ConnectionAwareSessionCleanupService<string>)
            };

            foreach (System.Type serviceType in serviceTypes)
                Assert.That(serviceType.GetConstructors(), Is.Empty, serviceType.FullName);
        }

        [Test]
        public void Create_RejectsInvalidMatchId()
        {
            Assert.Throws<System.ArgumentException>(() => CoreComposition.Create(default));
            Assert.That(
                typeof(CoreComposition).GetMethod("Create", System.Type.EmptyTypes),
                Is.Null,
                "A composition must not be constructible without an explicit MatchId.");
        }

        [Test]
        public void Create_ProvidesStableMatchScopedBetrayalLedger()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));

            Assert.That(composition.BetrayalLedger, Is.Not.Null);
            Assert.That(composition.BetrayalLedger, Is.SameAs(composition.BetrayalLedger));
        }

        [Test]
        public void SeparateCompositions_IsolateBetrayalLedgersEvenForDifferentMatchIds()
        {
            CoreComposition first = CoreComposition.Create(new MatchId(42));
            CoreComposition second = CoreComposition.Create(new MatchId(43));

            Assert.That(first.MatchId, Is.EqualTo(new MatchId(42)));
            Assert.That(second.MatchId, Is.EqualTo(new MatchId(43)));
            Assert.That(first.BetrayalLedger, Is.Not.SameAs(second.BetrayalLedger));
        }

        [Test]
        public void CreateVitalityAccount_UsesRequestedIdentityAndMaximum()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var account = composition.CreateVitalityAccount(new ActorId(7), 150);

            Assert.That(account.MatchId, Is.EqualTo(composition.MatchId));
            Assert.That(account.PlayerId, Is.EqualTo(new ActorId(7)));
            Assert.That(account.Current, Is.EqualTo(150u));
            Assert.That(account.Maximum, Is.EqualTo(150u));
            Assert.That(account.State, Is.EqualTo(PlayerLifecycleState.Alive));
        }

        [Test]
        public void CreateSessionRegistry_UsesExplicitCapacity()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var registry = composition.CreateSessionRegistry(16);

            Assert.That(registry.Capacity, Is.EqualTo(16));
            Assert.That(registry.Count, Is.Zero);
        }

        [Test]
        public void CreateConnectionSessionRegistry_UsesExplicitCapacityAndProvidedSessions()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IConnectionSessionRegistry connections = composition.CreateConnectionSessionRegistry(sessions, 12);

            Assert.That(connections.Capacity, Is.EqualTo(12));
            Assert.That(connections.Count, Is.Zero);
        }

        [Test]
        public void CreateSessionLifecycle_UsesRequestedSessionId()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var session = composition.CreateSessionLifecycle(new SessionId(77));

            Assert.That(session.SessionId, Is.EqualTo(new SessionId(77)));
            Assert.That(session.State, Is.EqualTo(SessionState.Pending));
        }

        [Test]
        public void CreateMatchPlayerRegistry_UsesCompositionMatchIdAndCapacity()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(8));
            var registry = composition.CreateMatchPlayerRegistry(12);

            Assert.That(registry.MatchId, Is.EqualTo(new MatchId(8)));
            Assert.That(registry.Capacity, Is.EqualTo(12));
            Assert.That(registry.Count, Is.Zero);
        }

        [Test]
        public void CreateCommandAdmissionService_UsesProvidedSessionRegistryAndQueue()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            ICommandAdmissionService<string> service = composition.CreateCommandAdmissionService(sessions, queue);

            var session = composition.CreateSessionLifecycle(new SessionId(10));
            sessions.Register(session);
            session.TryAuthenticate(new ActorId(1));
            var envelope = new CommandEnvelope<string>(session.SessionId, new CommandSequenceId(1), new ServerTick(0), "command");

            Assert.That(service.TryAdmit(envelope).IsSuccess, Is.True);
            Assert.That(queue.PendingCount, Is.EqualTo(1));
        }

        [Test]
        public void CreateMatchCommandAdmissionService_RejectsQueueFromAnotherMatch()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            CoreComposition otherComposition = CoreComposition.Create(new MatchId(43));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchPlayerRegistry players = composition.CreateMatchPlayerRegistry(4);
            CommandAdmissionQueue<string> foreignQueue =
                otherComposition.CreateCommandAdmissionQueue<string>(8, 4, 8);

            Assert.Throws<System.ArgumentException>(() =>
                composition.CreateMatchCommandAdmissionService(sessions, players, foreignQueue));
        }

        [Test]
        public void CreateMatchCommandAdmissionService_UsesProvidedSessionRegistryAndMatchRoster()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            var players = composition.CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);

            IMatchCommandAdmissionService<string> service =
                composition.CreateMatchCommandAdmissionService(sessions, players, queue);

            Assert.That(service.MatchId, Is.EqualTo(new MatchId(42)));
            Assert.That(queue.PendingCount, Is.Zero);
        }

        [Test]
        public void CoreComposition_DoesNotExposeAdmissionOnlyFactoriesAsPublicIngress()
        {
            Assert.That(
                typeof(CoreComposition).GetMethod("CreateCommandAdmissionService"),
                Is.Null,
                "Session-only admission must not be exposed as a public gameplay ingress.");

            Assert.That(
                typeof(CoreComposition).GetMethod("CreateMatchCommandAdmissionService"),
                Is.Null,
                "Membership-only admission must not bypass the explicit gameplay authorization policy.");
        }

        [Test]
        public void CreateMatchCommandAuthorizationService_UsesProvidedMatchAndPolicy()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchLifecycle match = composition.CreateMatchLifecycle();
            IMatchPlayerRegistry players = composition.CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            var policy = new AllowAllTestPolicy();

            IMatchCommandAuthorizationService<string> service =
                composition.CreateMatchCommandAuthorizationService(sessions, match, players, queue, policy);

            Assert.That(service.MatchId, Is.EqualTo(new MatchId(42)));
        }

        [Test]
        public void CreateMatchCommandAuthorizationService_RejectsMismatchedMatchScopes()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchLifecycle match = composition.CreateMatchLifecycle();
            IMatchPlayerRegistry players = CoreComposition.Create(new MatchId(43)).CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            var policy = new AllowAllTestPolicy();

            Assert.Throws<System.ArgumentException>(() =>
                composition.CreateMatchCommandAuthorizationService(sessions, match, players, queue, policy));
        }

        [Test]
        public void CreateMatchCommandAuthorizationService_RequiresExplicitPolicy()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchLifecycle match = composition.CreateMatchLifecycle();
            IMatchPlayerRegistry players = composition.CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);

            Assert.Throws<System.ArgumentNullException>(() =>
                composition.CreateMatchCommandAuthorizationService<string>(sessions, match, players, queue, null!));
        }

        [Test]
        public void CreateMatchCommandPayloadValidationService_UsesExplicitAuthorizationAndValidator()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchLifecycle match = composition.CreateMatchLifecycle();
            IMatchPlayerRegistry players = composition.CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            IMatchCommandAuthorizationService<string> authorization =
                composition.CreateMatchCommandAuthorizationService(sessions, match, players, queue, new AllowAllTestPolicy());

            IMatchCommandPayloadValidationService<string> validation =
                composition.CreateMatchCommandPayloadValidationService(authorization, new AllowAllTestValidator());

            Assert.That(validation.MatchId, Is.EqualTo(new MatchId(42)));
        }

        [Test]
        public void CreateMatchCommandPayloadValidationService_RequiresExplicitValidator()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            IMatchLifecycle match = composition.CreateMatchLifecycle();
            IMatchPlayerRegistry players = composition.CreateMatchPlayerRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            IMatchCommandAuthorizationService<string> authorization =
                composition.CreateMatchCommandAuthorizationService(sessions, match, players, queue, new AllowAllTestPolicy());

            Assert.Throws<System.ArgumentNullException>(() =>
                composition.CreateMatchCommandPayloadValidationService<string>(authorization, null!));
        }

        [Test]
        public void CreateSessionCleanupService_UsesProvidedDependencies()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var sessions = composition.CreateSessionRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            IConnectionSessionRegistry connections = composition.CreateConnectionSessionRegistry(sessions, 4);
            ISessionCleanupService<string> cleanup = composition.CreateSessionCleanupService(sessions, queue, connections);
            var session = composition.CreateSessionLifecycle(new SessionId(20));
            sessions.Register(session);
            session.TryClose();

            Assert.That(cleanup.TryRemoveClosedSession(session.SessionId).IsSuccess, Is.True);
            Assert.That(sessions.Get(session.SessionId).ErrorCode, Is.EqualTo(CoreErrorCode.NotFound));
        }

        [Test]
        public void CreateSessionCleanupService_RejectsConnectionRegistryFromDifferentSessionRegistry()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var cleanupSessions = composition.CreateSessionRegistry(4);
            var otherSessions = composition.CreateSessionRegistry(4);
            var queue = composition.CreateCommandAdmissionQueue<string>(8, 4, 8);
            IConnectionSessionRegistry connections = composition.CreateConnectionSessionRegistry(otherSessions, 4);

            Assert.Throws<System.ArgumentException>(() =>
                composition.CreateSessionCleanupService(cleanupSessions, queue, connections));
        }

        [Test]
        public void CreateCommandAdmissionQueue_UsesExplicitBounds()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(42));
            var queue = composition.CreateCommandAdmissionQueue<string>(12, 16, 32);

            Assert.That(queue.Capacity, Is.EqualTo(32));
            Assert.That(queue.PendingCount, Is.Zero);
        }

        [Test]
        public void CreateMatchLifecycle_UsesCompositionMatchId()
        {
            CoreComposition composition = CoreComposition.Create(new MatchId(9));
            var match = composition.CreateMatchLifecycle();

            Assert.That(match.MatchId, Is.EqualTo(new MatchId(9)));
            Assert.That(match.Phase, Is.EqualTo(MatchPhase.Created));
        }
    }
}
