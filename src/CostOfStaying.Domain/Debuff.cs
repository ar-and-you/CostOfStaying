using System;

namespace CostOfStaying.Domain
{
    /// <summary>
    /// A negative effect with a value, a remaining duration and a transferability
    /// flag (GLOSSARY: Debuff). Transfer moves the whole debuff including its
    /// remaining duration and decay contribution (RULE-02: Move, not Copy).
    /// </summary>
    public sealed class Debuff
    {
        public string Id { get; }

        /// <summary>Extra vitality decay per second while this debuff is active.</summary>
        public double DecayBonusPerSecond { get; }

        /// <summary>Seconds remaining. Decremented by the server tick.</summary>
        public double RemainingSeconds { get; internal set; }

        /// <summary>Whether this debuff can be moved by Suffering Transfer.</summary>
        public bool Transferable { get; }

        public Debuff(string id, double decayBonusPerSecond, double durationSeconds, bool transferable)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Debuff id must not be null, empty, or whitespace.", nameof(id));
            if (!IsFinite(decayBonusPerSecond) || decayBonusPerSecond < 0)
                throw new ArgumentOutOfRangeException(nameof(decayBonusPerSecond), decayBonusPerSecond, "Must be finite and non-negative.");
            if (!IsFinite(durationSeconds) || durationSeconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), durationSeconds, "Must be finite and positive.");

            Id = id;
            DecayBonusPerSecond = decayBonusPerSecond;
            RemainingSeconds = durationSeconds;
            Transferable = transferable;
        }

        private static bool IsFinite(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        internal Debuff Clone()
        {
            return new Debuff(Id, DecayBonusPerSecond, RemainingSeconds, Transferable);
        }
    }
}
