using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowAutomation.Harness;

namespace PersonalLoansTests
{
    [RequiresSTA]
    public class DataSetup : PersonalLoansWorkflowTestBase<BasePage>
    {
        /// <summary>
        /// This test will create the test data in the Personal Loans Workflow
        /// </summary>
        [Test, TestCaseSource(typeof(DataSetup), "GetWorkflows")]
        public void CreateCases(Common.Enums.WorkflowEnum workflow)
        {
            IDataCreationHarness harness = new DataCreationHarness();
            var casesAtCorrectStage = harness.CreateTestCases(workflow);
            Assert.True(casesAtCorrectStage, "Cases are not at the correct stage");
        }

        public List<Common.Enums.WorkflowEnum> GetWorkflows()
        {
            return new List<Common.Enums.WorkflowEnum> { Common.Enums.WorkflowEnum.PersonalLoans };
        }
    }
}