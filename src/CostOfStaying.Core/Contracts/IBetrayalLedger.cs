namespace CostOfStaying.Core.Contracts
{
    using CostOfStaying.Core.Domain;

    /// <summary>
    /// Read-only view of the server-owned betrayal ledger.
    /// Event recording is intentionally not exposed through the public contract:
    /// only trusted Core gameplay code may append events after membership and
    /// gameplay-authority checks have completed.
    /// </summary>
    public interface IBetrayalLedger
    {
        ulong ScoreFor(ActorId actorId);
    }
}
