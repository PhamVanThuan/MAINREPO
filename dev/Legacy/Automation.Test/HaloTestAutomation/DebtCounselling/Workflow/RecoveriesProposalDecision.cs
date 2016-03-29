using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class RecoveriesProposalDecisionTests : TestBase<WorkflowYesNo>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.DebtCounsellingSupervisor);
        }

        protected override void OnTestStart()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision, TestUsers.DebtCounsellingSupervisor);
        }

        /// <summary>
        /// This test performs the Approve Shortfall action. It ensures that the Debt Counselling Supervisor can approve the Recoveries Proposal and checks that
        /// the case is sent back to the Debt Counselling Consultant at the Pend Cancellation state.
        /// </summary>
        [Test]
        public void ApproveRecoveriesProposal()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ApproveShortfall);
            base.View.Confirm(true, false);
            //Assertions
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendCancellation);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ApproveShortfall);
        }

        /// <summary>
        /// This test performs the Decline Shortfall action. It ensures that the Debt Counselling Supervisor can decline the Recoveries Proposal and checks that
        /// the case is sent back to the Debt Counselling Consultant at the Pend Cancellation state.
        /// </summary>
        [Test]
        public void DeclineRecoveriesProposal()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.DeclineShortfall);
            base.View.Confirm(true, false);
            //Assertions
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendCancellation);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DeclineShortfall);
        }
    }
}