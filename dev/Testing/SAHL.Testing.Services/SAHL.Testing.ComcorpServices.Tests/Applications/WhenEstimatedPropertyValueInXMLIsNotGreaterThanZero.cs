using NUnit.Framework;
using SAHL.Testing.ComcorpServices.Tests;
namespace SAHL.Testing.ComcorpServices.Tests.Applications
{
    [TestFixture]
    [ExpectError("Estimated Market Value of the Home must be greater than R 0.00")]
    public class WhenEstimatedPropertyValueInXMLIsNotGreaterThanZero : ComCorpApplicationTest
    {
    }
}