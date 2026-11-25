using System;
using CostOfStaying.Domain;
using NUnit.Framework;

namespace CostOfStaying.Domain.Tests
{
    [TestFixture]
    public class DebuffTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Constructor_Rejects_Blank_Ids(string id)
        {
            Assert.Throws<ArgumentException>(() =>
                new Debuff(id, decayBonusPerSecond: 0.1, durationSeconds: 1.0, transferable: true));
        }

        [TestCase(-0.1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Constructor_Rejects_Invalid_Decay_Bonuses(double bonus)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Debuff("valid-id", bonus, durationSeconds: 1.0, transferable: true));
        }

        [TestCase(0.0)]
        [TestCase(-1.0)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Constructor_Rejects_Invalid_Durations(double duration)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Debuff("valid-id", decayBonusPerSecond: 0.1, duration, transferable: true));
        }

        [Test]
        public void Constructor_Accepts_Zero_Decay_Bonus_With_Positive_Duration()
        {
            var debuff = new Debuff("valid-id", decayBonusPerSecond: 0, durationSeconds: 1, transferable: false);

            Assert.That(debuff.DecayBonusPerSecond, Is.EqualTo(0));
            Assert.That(debuff.RemainingSeconds, Is.EqualTo(1));
            Assert.That(debuff.Transferable, Is.False);
        }
    }
}
