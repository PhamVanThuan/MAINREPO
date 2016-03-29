using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class CourtApplicationWithdrawnTests : TestBase<WorkflowYesNo>
    {
        private string userForTest = TestUsers.DebtCounsellingCourtConsultant;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(userForTest);
        }

        [Test]
        public void CourtApplicationWithdrawn()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendCourtDecision, userForTest);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CourtApplicationWithdrawn);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(Common.Enums.WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Common.Constants.Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.View.Confirm(true, true);
            //check all court details are inactive
            DebtCounsellingAssertions.AssertActiveCourtDetailsDoNotExist(base.TestCase.DebtCounsellingKey);
            //check court case indicator set to false
            DebtCounsellingAssertions.AssertCourtCaseIndicator(base.TestCase.DebtCounsellingKey, false);
            //check case is at Manage Proposal state
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
            //check case is assigned to consultant
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, false);
            //check for the stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_CourtApplicationWithdrawn);
        }
    }
}