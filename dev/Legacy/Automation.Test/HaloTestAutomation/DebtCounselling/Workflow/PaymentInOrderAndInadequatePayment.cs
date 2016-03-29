using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class PaymentInOrderAndInadequatePayment : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
        }

        #region InadequatePaymentTests/PaymentInOrder

        /// <summary>
        /// This test will perform the Inadequate Payment action on a workflow case that is at the Payment Review state. This will move the case back to the
        /// Pend Payment state and assign it back to a Consultant.
        /// </summary>
        [Test, Description(@"This test will perform the Inadequate Payment action on a workflow case that is at the Payment Review state. This will move the case back to the
        Pend Payment state and assign it back to a Consultant.")]
        public void InadequatePayment()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PaymentReview, TestUsers.DebtCounsellingSupervisor);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.InadequatePayment);
            //we need the dcc user
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendPayment);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_InadequatePayment);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
        }

        /// <summary>
        /// This test will perform the Payment in Order action on a case at the Payment Review state, moving the case to the Debt Review Approved state.
        /// </summary>
        [Test, Description(@"This test will perform the Payment in Order action on a case at the Payment Review state, moving the case to the Debt Review Approved state.")]
        public void PaymentInOrder()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PaymentReview, TestUsers.DebtCounsellingSupervisor, hasArrearTransactions: true, searchForCase: false);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentinOrder);
            //we need the dcc user
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.DebtReviewApproved);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_PaymentinOrder);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, false);
            //we need to get the term review date for the timer assertion
            string reviewDate = Service<IProposalService>().GetReviewDateOfAcceptedProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal);
            //check the scheduled activity is setup
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling.Termexpired, 0, 0, 0, customScheduledActivityDate: reviewDate);
            //check that the loan transaction exists
            TransactionAssertions.AssertArrearTransactionExists(base.TestCase.AccountKey, TransactionTypeEnum.DebtReviewArrangementCredit);
        }

        #endregion InadequatePaymentTests/PaymentInOrder
    }
}