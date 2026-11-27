namespace CostOfStaying.Domain
{
    /// <summary>
    /// Time source injected into the Domain aggregate. A production host should
    /// provide a monotonic clock; this interface does not authenticate a caller
    /// or establish a client/server trust boundary.
    /// </summary>
    public interface IMatchClock
    {
        double NowSeconds { get; }
    }

    /// <summary>Deterministic clock for tests and simulations.</summary>
    public sealed class ManualClock : IMatchClock
    {
        public double NowSeconds { get; private set; }

        public ManualClock(double startSeconds = 0.0)
        {
            NowSeconds = startSeconds;
        }

        public void Advance(double deltaSeconds)
        {
            NowSeconds += deltaSeconds;
        }

        public void Set(double seconds)
        {
            NowSeconds = seconds;
        }
    }

    /// <summary>
    /// Host-agnostic monotonic elapsed-time source. Uses Stopwatch so it is
    /// immune to wall-clock adjustments (NTP, user changes).
    /// </summary>
    public sealed class MonotonicClock : IMatchClock
    {
        private readonly System.Diagnostics.Stopwatch _stopwatch =
            System.Diagnostics.Stopwatch.StartNew();

        public double NowSeconds => _stopwatch.Elapsed.TotalSeconds;
    }
}
