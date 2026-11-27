using System;

namespace CostOfStaying.Domain
{
    /// <summary>
    /// Proposed client-intent DTOs for a future host/transport integration.
    /// These values are untrusted input to Match methods: the host must bind
    /// PlayerId to an authenticated actor and derive authoritative measurements.
    /// This Domain assembly does not implement authentication or transport.
    /// </summary>
    public abstract class ClientIntent
    {
        /// <summary>Claimed actor id; a host must bind it to the authenticated connection.</summary>
        public Guid PlayerId { get; set; }

        /// <summary>Client request identity; the Domain validates it for idempotency where supported.</summary>
        public string RequestId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>Client timestamp for diagnostics only; not authoritative game time.</summary>
        public double SentAtSeconds { get; set; }
    }

    public sealed class TransferIntent : ClientIntent
    {
        public Guid TargetId { get; set; }
        /// <summary>
        /// Client-claimed distance for diagnostics only. A host must derive
        /// actual distance from authoritative positions before calling Match.TryTransfer.
        /// </summary>
        public double ClaimedDistanceMeters { get; set; }
    }

    public sealed class VoteIntent : ClientIntent
    {
        public VoteChoice Choice { get; set; }
    }

    public sealed class HintIntent : ClientIntent
    {
        public string HintText { get; set; } = string.Empty;
    }

    public sealed class CollectEchoIntent : ClientIntent
    {
        public string EchoId { get; set; } = string.Empty;
    }
}
