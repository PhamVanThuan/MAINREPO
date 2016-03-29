using Meyn.TestLink;
using NUnit.Framework;

namespace LoanServicingTests.ClientSuperSearchTests
{
    using Helpers;

    [TestFixture, RequiresSTA]
#if !DEBUG
    [TestLinkFixture(
        Url = "http://sahls31:8181/testlink/lib/api/xmlrpc.php",
        ProjectName = "HALO Automation",
        TestPlan = "Regression Tests",
        TestSuite = "Loan Servicing Tests",
        UserId = "admin",
        DevKey = "896f902c0397d7c1849290a44d0f6fb5")]
#endif
    public sealed class AdvancedSearch
    {
        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
            BrowserHelpers.StartBrowser();
            BrowserHelpers.NavigateToClientSuperSearchAdvanced();
        }
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BrowserHelpers.CloseBrowser();
        }
    }
}
