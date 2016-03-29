using NUnit.Framework;
using SAHL.Testing.ComcorpServices.Tests;
namespace SAHL.Testing.ComcorpServices.Tests.Applications
{
    [TestFixture]
    [ExpectError("At least one contact detail (Email, Home, Work or Cell Number) is required.")]
    public class WhenApplicantContactDetailsAreMissing : ComCorpApplicationTest
    {
    }
}