using BuildingBlocks.Presenters.CommonPresenters;
using NUnit.Framework;
using WorkflowAutomation.Harness;

namespace CAP2Tests
{
    [RequiresSTA]
    public class CAP2DataSetup : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //Service<ICAP2Service>().CAP2AutomationSetup(TestUsers.ClintonS, TestUsers.CreditUnderwriter2);
        }

        [Test]
        public void CreateCAP2Offers()
        {
            IDataCreationHarness harness = new DataCreationHarness();
            bool casesCreatedSuccesfully = harness.CreateTestCases(Common.Enums.WorkflowEnum.CAP2Offers);
            Assert.That(casesCreatedSuccesfully, "Cases were not created correctly");
        }
    }
}