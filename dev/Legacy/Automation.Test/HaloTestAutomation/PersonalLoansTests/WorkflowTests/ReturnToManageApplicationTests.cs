using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class ReturnToManageApplicationTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
        }

        /// <summary>
        /// This test will return a case to the Manage Application state from the Credit state.
        /// </summary>
        [Test, Description("This test will return a case to the Manage Application state from the Credit state.")]
        public void ReturnToManageApplication()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReturntoManageApplication);
            //get the expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageApplication);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ReturnToManageApplication);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.ManageApplication, Workflows.PersonalLoans);
        }
    }
}