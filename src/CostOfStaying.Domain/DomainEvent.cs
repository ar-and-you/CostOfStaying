using System;

namespace CostOfStaying.Domain
{
    /// <summary>Domain event kinds emitted by the Match aggregate; a host adapter may replicate them.</summary>
    public enum DomainEventKind
    {
        MatchStarted,
        PlayerDied,
        CollapseApplied,
        DebuffTransferred,
        TransferRejected,
        BetrayalIncreased,
        VoteOpened,
        VoteCast,
        VoteClosed,
        EchoCollected,
        HintSent,
        MatchEnded
    }

    /// <summary>
    /// Immutable record emitted by a Domain state transition. A future host/network
    /// adapter may replicate it; this Domain assembly does not implement transport
    /// or authenticate the origin of an operation.
    /// </summary>
    public sealed class DomainEvent
    {
        public DomainEventKind Kind { get; }
        public double AtSeconds { get; }
        public Guid? PlayerId { get; }
        public Guid? TargetId { get; }
        public string? Detail { get; }

        public DomainEvent(DomainEventKind kind, double atSeconds, Guid? playerId = null, Guid? targetId = null, string? detail = null)
        {
            Kind = kind;
            AtSeconds = atSeconds;
            PlayerId = playerId;
            TargetId = targetId;
            Detail = detail;
        }
    }
}
