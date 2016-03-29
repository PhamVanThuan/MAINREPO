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
    public sealed class DefaultInPayment : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test will check that the debt counselling consultant is alerted when an account under debt counselling goes into arrears. This is done
        /// by raising the EXT Into Arrears flag for cases that have breached the arrear tolerance level. Once the flag fires the case will be sent to
        /// the Default in Payment state on the consultants worklist.
        /// </summary>
        [Test, Description(@"This test will check that the debt counselling consultant is alerted when an account under debt counselling goes into arrears.
        This is done by raising the EXT Into Arrears flag for cases that have breached the arrear tolerance level. Once the flag fires the case will be sent
        to the Default in Payment state on the consultants worklist.")]
        public void DefaultInPaymentIntoArrears()
        {
            //search for a case at Debt Review Approved
            base.StartTest(WorkflowStates.DebtCounsellingWF.DebtReviewApproved, TestUsers.DebtCounsellingConsultant);
            Service<IX2WorkflowService>().InsertActiveExternalActivity(Workflows.DebtCounselling, ExternalActivities.DebtCounselling.EXTIntoArrears, base.TestCase.InstanceID,
                base.TestCase.DebtCounsellingKey, WorkflowActivities.DebtCounselling.EXTIntoArrears);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.DefaultinPayment);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                        true, true);
        }

        /// <summary>
        /// This test will check that once the account is no longer in breach of the arrear tolerance level, the firing of the EXT Out of Arrears flag will
        /// send the case back to the Debt Review Approved state.
        /// </summary>
        [Test, Description(@"This test will check that once the account is no longer in breach of the arrear tolerance level, the firing of the EXT Out of
        Arrears flag will send the case back to the Debt Review Approved state.")]
        public void DefaultInPaymentOutOfArrears()
        {
            //search for a case at Default in Payment
            base.StartTest(WorkflowStates.DebtCounsellingWF.DefaultinPayment, TestUsers.DebtCounsellingConsultant);
            //we need to update the review date to avoid cases moving straight to Term Review
            Service<IProposalService>().UpdateReviewDateOfActiveAcceptedProposal(base.TestCase.DebtCounsellingKey);
            //fire the EXT Out of Arrears flag and assert that the case has moved to the Debt Review Approved state
            Service<IX2WorkflowService>().InsertActiveExternalActivity(Workflows.DebtCounselling, ExternalActivities.DebtCounselling.EXTOutofArrears, base.TestCase.InstanceID,
                base.TestCase.DebtCounsellingKey, WorkflowActivities.DebtCounselling.EXTOutofArrears);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.DebtReviewApproved);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                        true, false);
        }
    }
}