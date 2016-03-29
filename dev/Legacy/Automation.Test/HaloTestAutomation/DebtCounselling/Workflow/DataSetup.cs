using System.Text;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using BuildingBlocks;
using Meyn.TestLink;

namespace DebtCounselling.Workflow
{
       [TestFixture, RequiresSTA]
#if !DEBUG
    [TestLinkFixture(
        Url = "http://sahls31:8181/testlink/lib/api/xmlrpc.php",
        ProjectName = "Halo Automation",
        TestPlan = "Regression Tests",
        TestSuite = "Debt Counselling Data Setup",
        UserId = "admin",
        DevKey = "e5ff80e4c6dd5726f495d3389c3a228a")]
#endif   
    public sealed class DataSetup
    {
        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
        }

        [SetUp]
        public void TestStartUp()
        {
        }

        [TearDown]
        public void TestCleanUp()
        {
        }

        [TestFixtureTearDown]
        public void TestSuiteCleanUp()
        {
        }
    }
}
