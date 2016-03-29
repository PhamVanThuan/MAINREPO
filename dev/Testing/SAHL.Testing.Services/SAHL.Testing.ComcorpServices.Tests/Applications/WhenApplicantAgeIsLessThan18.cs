using NUnit.Framework;
using SAHL.Testing.ComcorpServices.Tests;
namespace SAHL.Testing.ComcorpServices.Tests.Applications
{
    [TestFixture]
    [ExpectError("Model Validation error : CHUCK BASS : Applicant should be older than 18.")]
    public class WhenApplicantAgeIsLessThan18 : ComCorpApplicationTest
    {
    }
}