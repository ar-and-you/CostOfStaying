namespace CostOfStaying.Core.Tests
{
    using System.Reflection;
    using CostOfStaying.Core.Contracts;
    using CostOfStaying.Core.Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class BetrayalLedgerApiSurfaceTests
    {
        [Test]
        public void PublicLedgerContract_IsReadOnly()
        {
            Assert.That(
                typeof(IBetrayalLedger).GetMethod("TryRecord"),
                Is.Null,
                "External consumers must not receive a public event-recording capability.");
        }

        [Test]
        public void ConcreteLedger_DoesNotExposePublicMutation()
        {
            MethodInfo? publicMutation = typeof(BetrayalLedger).GetMethod(
                "TryRecord",
                BindingFlags.Instance | BindingFlags.Public);

            Assert.That(
                publicMutation,
                Is.Null,
                "Casting the public ledger view to its concrete type must not expose mutation.");
        }
    }
}
