using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public sealed class TimerExpiredAndContinueTerm : TestBase<BasePage>
    {
        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            FireTimer(5);
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// tries to find 5 cases belonging to DCCUser at the Debt Review Approved state and fires the timer on these cases to move them to the Term Review state.
        /// </summary>
        private void FireTimer(int repeat)
        {
            Automation.DataModels.DebtCounselling testCase;
            for (int i = 0; i < repeat; i++)
            {
                testCase = WorkflowHelper.SearchForCase(WorkflowStates.DebtCounsellingWF.DebtReviewApproved, TestUsers.DebtCounsellingConsultant);
                if (testCase.DebtCounsellingKey != 0)
                {
                    base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.FireTermExpiredTimer, testCase.DebtCounsellingKey);
                }
                i++;
            }
        }

        #endregion SetupTearDown

        #region Tests

        /// <summary>
        /// This test will find a case at the Debt Review Approved state and update the timer in order to make it fire. After it has fired we check that the case
        /// has moved to the Term Review state and is correctly assigned to the consultant.
        /// </summary>
        [Test, Description(@"This test will find a case at the Debt Review Approved state and fire the timer checking that the case has moved to the Term Review state and is correctly assigned to the consultant.")]
        public void TermExpiredTimer()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DebtReviewApproved, TestUsers.DebtCounsellingConsultant);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.FireTermExpiredTimer, base.TestCase.DebtCounsellingKey);
            //check that the case has changed states
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.TermReview);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ReviewTerm);
        }

        /// <summary>
        /// This test will complete the Continue another Term action moving the case back to the Debt Review Approved state. It will ensure that the proposal review date
        /// gets updated and that the timer is created using the review date provided. It also checks that the case moves states and that the stage transition is
        /// written.
        /// </summary>
        [Test, Description(@"This test will complete the Continue another Term action moving the case back to the Debt Review Approved state. It will ensure that the
		proposal review date gets updated and that the timer is created using the review date provided. It also checks that the case moves states and that the stage
		transition is written.")]
        public void ContinueAnotherTerm()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.TermReview, TestUsers.DebtCounsellingConsultant);
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Continueanotherterm);
            base.Browser.Page<TermReviewUpdate>().EnterValidReviewDate();
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.DebtReviewApproved);
            string reviewDate = Service<IProposalService>().GetReviewDateOfAcceptedProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.Proposal);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, false);
            //check the scheduled activity is setup
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling.Termexpired, 0, 0, 0, customScheduledActivityDate: reviewDate);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_CurrentProposalTermExtended);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.DebtReviewApproved);
        }

        /// <summary>
        /// This test will try and complete the Continue Another Term action providing a review date that is more than 18 months in the future. The user
        /// should not be allowed to complete the action and should be provided with a warning message.
        /// </summary>
        [Test, Description(@"This test will try and complete the Continue Another Term action providing a review date that is more than 18 months in the future. The user
		should not be allowed to complete the action and should be provided with a warning message.")]
        public void ContinueAnotherTermReviewDateGreaterThan18Months()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.TermReview, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Continueanotherterm);
            base.Browser.Page<TermReviewUpdate>().EnterReviewDateGreaterThan18Months();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The Review Date cannot be more than 18 months from today. The latest possible date is {0}.", DateTime.Now.AddMonths(18).ToString(Formats.DateFormat)));
            //case is in the same state
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.TermReview);
        }

        /// <summary>
        /// This test will try and complete the Continue Another Term action providing a review date that is less than today. The user should not be allowed to complete
        /// the action and should be provided with a warning message.
        /// </summary>
        [Test, Description(@"This test will try and complete the Continue Another Term action providing a review date that is less than today. The user should not be allowed
		to complete the action and should be provided with a warning message.")]
        public void ContinueAnotherTermReviewDateLessThanToday()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.TermReview, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Continueanotherterm);
            base.Browser.Page<TermReviewUpdate>().EnterReviewDateLessThanToday();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Review Date may not be a date in the past.");
            //case is in the same state
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.TermReview);
        }

        /// <summary>
        /// The Review Date is mandatory. The user should be provided with a warning if they enter a blank value.
        /// </summary>
        [Test, Description("The Review Date is mandatory. The user should be provided with a warning if they enter a blank value.")]
        public void ContinueAnotherTermMandatoryReviewDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.TermReview, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Continueanotherterm);
            base.Browser.Page<TermReviewUpdate>().EnterBlankReviewDate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Review Date is Mandatory");
            //case is in the same state
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.TermReview);
        }

        #endregion Tests
    }
}