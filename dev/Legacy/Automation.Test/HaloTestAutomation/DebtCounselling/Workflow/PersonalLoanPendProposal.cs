using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public sealed class RespondToDebtCounsellorTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingAdmin);
        }

        #region Tests

        /// <summary>
        /// This test ensures that a Personal Loan, when placed in Debt Counselling, ends up in a 'Personal Loan Pend Proposal' state.
        /// </summary>
        [Test, Description(@"This test ensures that a Personal Loan, when placed in Debt Counselling, ends up in a 'Personal Loan Pend Proposal' state once the Respond to Debt Counsellor activity has been performed")]
        public void RespondToDebtCounsellorOnAPersonalLoanCase()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, eWorkCase: false, hasArrearTransactions: false, product: ProductEnum.PersonalLoan, searchForCase: false);
            PerformRespondToDebtCounsellor();
            string expectedConsultant = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            // Assert that the case is at state 'Personal Loan Pend Proposal'
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.PersonalLoanPendProposal);
            // Assert that only one active WorkflowRole record exists for an ADUser on the case
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, expectedConsultant);
            // Assert that the latest workflow role assignment record has been correctly written for an instance
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, expectedConsultant, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
        }

        /// <summary>
        /// Once the case enters the Pend Proposal state an clone instance is created for the case. This clone should have a 60 day timer attached to
        /// it once it has been created which will fired 60 business days after the clone has been created.
        /// </summary>
        [Test, Description(@"Once the case enters the Pend Proposal state an clone instance is created for the case. This clone should have a 60 day timer attached to it once it has been created.")]
        public void RespondToDebtCounsellor()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, searchForCase: false);
            PerformRespondToDebtCounsellor();
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.PendProposal);
            //check clone exists at the correct state
            base.TestCase.ClonedInstanceID = X2Assertions.AssertX2CloneExists(base.TestCase.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF._60DayTimerHold, Workflows.DebtCounselling);
            //check the timer value, should be 60 days from the 17.1 date, which in the case of our automation test is today.
            var stageTransition = Service<IStageTransitionService>().GetLatestStageTransitionByGenericKeyAndSDSDGKey(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_1Received);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._60days, 60, true, stageTransition.EndTransitionDate);
            //fire the timer
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire60DayTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._60days, base.TestCase.ClonedInstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling.NoCourtDateorDeposit, base.TestCase.InstanceID, 1);
            //our case should now be at Intent to Terminate State and the clone should be archived.
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.IntenttoTerminate);
            //the clone should be archived
            X2Assertions.AssertCurrentX2State(base.TestCase.ClonedInstanceID, WorkflowStates.DebtCounsellingWF.Archive60days);
            //we should also have a 5 day timer created
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling.Fivedays, 5, true);
        }

        /// <summary>
        /// Completes the Respond to Debt Counsellor Action
        /// </summary>
        private void PerformRespondToDebtCounsellor()
        {
            int legalEntityKey = Service<IDebtCounsellingService>().GetDCTestCaseDebtCounsellorCorrespondenceDetails(null, base.TestCase.AccountKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.RespondtoDebtCounsellor);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectCorrespondenceRecipient(legalEntityKey);
        }

        #endregion Tests
    }
}