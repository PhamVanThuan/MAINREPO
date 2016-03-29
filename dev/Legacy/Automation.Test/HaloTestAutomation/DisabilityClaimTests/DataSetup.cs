using BuildingBlocks.Presenters.CommonPresenters;
using NUnit.Framework;
using System.Collections.Generic;
using WorkflowAutomation.Harness;

namespace DisabilityClaimTests
{
    [RequiresSTA]
    public class DataSetup : DisabilityClaimsWorkflowTestBase<BasePage>
    {
        [Test, TestCaseSource(typeof(DataSetup), "GetWorkflows")]
        public void CreateCases(Common.Enums.WorkflowEnum workflow)
        {
            IDataCreationHarness harness = new DataCreationHarness();
            var casesAtCorrectStage = harness.CreateTestCases(workflow);
            Assert.True(casesAtCorrectStage, "Cases are not at the correct stage");
        }

        public List<Common.Enums.WorkflowEnum> GetWorkflows()
        {
            return new List<Common.Enums.WorkflowEnum> { Common.Enums.WorkflowEnum.DisabilityClaim };
        }
    }
}