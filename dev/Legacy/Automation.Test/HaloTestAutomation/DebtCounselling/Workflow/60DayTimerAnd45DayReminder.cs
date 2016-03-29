using System;
using System.Threading;
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
    public sealed class SixtyDayTimerAnd45DayReminder : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        #region Tests

        /// <summary>
        /// Should there be no court details of types 'Court/Tribunal - Court Order Granted' or 'Court/Tribunal - Court Application' then the 5 day timer should fire
        /// and move the workflow case to the Termination state. A termination reason is automatically added to the debt counselling case.
        /// </summary>
        [Test, Description(@"Should there be no court details of types 'Court/Tribunal - Court Order Granted' or 'Court/Tribunal - Court Application' then the 5 day timer
				should fire and move the workflow case to the Termination state. A termination reason is automatically added to the debt counselling case. ")]
        public void FireFiveDayTimeWhenNoCourtDetailsExistsMovesToTerminationState()
        {
            //we need to update the scheduled activity
            base.StartTest(WorkflowStates.DebtCounsellingWF.IntenttoTerminate, TestUsers.DebtCounsellingConsultant);
            //remove court details
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            //fire the timer
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire5DaysTimer, base.TestCase.DebtCounsellingKey);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.Termination);
            //reason should be added to our case
            ReasonAssertions.AssertReason(ReasonDescription._86_10_Noarrangement_notreferredtocourtandnopaymentsintermsofproposal, ReasonType.DebtCounsellingTermination,
                base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey);
        }

        /// <summary>
        /// This test will create a case and ensure that the instance clone is created. Prior to updating the scheduled activity the test will insert court details against
        /// the debt counselling case. This should result in the case being moved to the 60 days review state.
        /// </summary>
        [Test, Description(@"This test will create a case and ensure that the instance clone is created. Prior to updating the scheduled activity the test will insert court
				details against the debt counselling case. This should result in the case being moved to the 60 days review state.")]
        public void SixtyDayTimerFiresWithCourtDetailsCaseMovesTo60DaysReview()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            //check clone exists at the correct state
            base.TestCase.ClonedInstanceID = X2Assertions.AssertX2CloneExists(TestCase.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF._60DayTimerHold, Workflows.DebtCounselling);
            //we need to insert court details
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtCourtApplication, DateTime.Now.AddDays(5));
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire60DayTimer, base.TestCase.DebtCounsellingKey);
            //wait for the timer
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._60days, base.TestCase.ClonedInstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling.CourtDateOrDeposit, base.TestCase.InstanceID, 1);
            //our case should now be at 60 Days Review State and the clone should be archived.
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF._60daysReview);
            //the clone should be archived
            X2Assertions.AssertCurrentX2State(base.TestCase.ClonedInstanceID, WorkflowStates.DebtCounsellingWF.Archive60days);
        }

        /// <summary>
        /// Once the case is at the previous state, the user has the ability to return the case to the state from which the flag was raised.
        /// </summary>
        [Test, Description("Once the case is at the previous state, the user has the ability to return the case to the state from which the flag was raised.")]
        public void SixtyDayReviewReturnToPreviousState()
        {
            //start the browser
            base.StartTest(WorkflowStates.DebtCounsellingWF._60daysReview, TestUsers.DebtCounsellingConsultant);
            var previousState = Service<IX2WorkflowService>().GetPreviousStateForInstancePriorToActivity(base.TestCase.InstanceID, WorkflowActivities.DebtCounselling.EXT_60daynodateorpay);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ContinueDebtCounselling);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //we should now be at the previous state i.e. Pend Proposal
            Thread.Sleep(5000);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, previousState);
        }

        /// <summary>
        /// This test will create a case and ensure that the instance clone is created. Prior to updating the scheduled activity the test will post an instalment payment
        //deposit against the account, which should result in the case being moved to the 60 DAYS REVIEW state.
        /// </summary>
        [Test, Description(@"This test will create a case and ensure that the instance clone is created. Prior to updating the scheduled activity the test will post an
		instalment payment deposit against the account, which should result in the case being moved to the 60 DAYS REVIEW state. ")]
        public void SixtyDayTimerInstalmentPaymentDeposit17pt1BeforeTransactionDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //check clone exists at the correct state
            base.TestCase.ClonedInstanceID = X2Assertions.AssertX2CloneExists(TestCase.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF._60DayTimerHold, Workflows.DebtCounselling);
            var financialService = Service<IAccountService>().GetOpenFinancialServiceRecordByType(base.TestCase.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            //we need to insert court details
            Service<ILoanTransactionService>().pProcessTran(financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<Int32>(), TransactionTypeEnum.InstalmentPaymentDeposit, 2000.00M, "Debt Counselling Test",
                TestUsers.HaloUser);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire60DayTimer, base.TestCase.DebtCounsellingKey);
            //wait for the scheduled activity
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._60days, base.TestCase.ClonedInstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling.CourtDateOrDeposit, base.TestCase.InstanceID, 1);
            //our case should now be at Intent to Terminate State and the clone should be archived.
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF._60daysReview);
            //the clone should be archived
            X2Assertions.AssertCurrentX2State(base.TestCase.ClonedInstanceID, WorkflowStates.DebtCounsellingWF.Archive60days);
        }

        /// <summary>
        /// This test will create a debt counselling case and ensure that the 60 day timer clone and scheduled activity is setup. The scheduled activity will then
        /// be updated in order to move the case to the INTENT TO TERMINATE state. At this point Court Details are added to the debt counselling case and the 5 Day Timer
        /// is updated to fire the scheduled activity. Due to the court details being added, the timer should not complete and the case should not move.
        /// </summary>
        [Test, Sequential, Description(@"This test will create a debt counselling case and ensure that the 60 day timer clone and scheduled activity is setup. The scheduled activity
				will then be updated in order to move the case to the INTENT TO TERMINATE state. At this point Court Details are added to the debt counselling case and the 5 Day
				Timer is updated to fire the scheduled activity. Due to the court details being added, the timer should not complete and the case should not move.")]
        public void FiveDayTimerShouldNotFireWhenCourtDetailsExist(
            [Values(HearingTypeEnum.Court, HearingTypeEnum.Tribunal)] HearingTypeEnum hearingType,
            [Values(HearingAppearanceTypeEnum.CourtCourtApplication, HearingAppearanceTypeEnum.TribunalCourtApplication)] HearingAppearanceTypeEnum appearanceType)            
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.IntenttoTerminate, TestUsers.DebtCounsellingConsultant);
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, hearingType, appearanceType, DateTime.Now.AddDays(5));
            try
            {
                var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire5DaysTimer, base.TestCase.DebtCounsellingKey);
            }
            catch (Exception ex)
            {
                Assert.That(ex.ToString().Contains("The 5 Days timer cannot fire as the applicable Hearing Appearance Types exists on the Debt Counselling Case."),
                        "OnStart() failed for an unexpected Reason");
            }
        }

        /// <summary>
        /// This test will create a case and ensure that the 45 Day Reminder timer is correctly created once the case has been moved to the Pend Proposal state.
        /// The timer is then updated to fire and the test ensures that the case is moved to the 45 Day Reminder state. Once at this state the 45 Day Reminder
        /// Sent action is completed moving the case back to the `Pend Proposal` state.
        /// </summary>
        [Test, Description(@"This test will create a case and ensure that the 45 Day Reminder timer is correctly created once the case has been moved to the Pend Proposal
				state. The timer is then updated to fire and the test ensures that the case is moved to the 45 Day Reminder state. Once at this state the 45 Day Reminder
				Sent action is completed moving the case back to the Pend Proposal state.")]
        public void FortyFiverDayReminderSent()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._45Days, 45, true);
            //perform the activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire45DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._45Days, base.TestCase.InstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling._45DaysLapsed, base.TestCase.InstanceID, 1);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF._45DayReminder);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_45DayReminder);
            //load the case again
            base.LoadCase(WorkflowStates.DebtCounsellingWF._45DayReminder);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling._45dayremindersent);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case is back at Pend Proposal
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendProposal);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
        }

        /// <summary>
        /// This test will ensure that the 45 Day Timer DOES NOT fire if the case at the Pend Proposal state has court details capture against it.
        /// </summary>
        [Test, Description(@"This test will ensure that the 45 Day Timer DOES NOT fire if the case at the Pend Proposal state has court details capture against it.")]
        public void WhenCourtDetailsExistFortyFiveTimerShouldNotMoveCase()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._45Days, 45, true);
            //we need to insert court details
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtCourtApplication, DateTime.Now.AddDays(5));
            //perform the activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire45DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._45Days, base.TestCase.InstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling._45DaysCancel, base.TestCase.InstanceID, 1);
            //case shouldnt have moved
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendProposal);
        }

        /// <summary>
        /// This test will create a case and send the case through to the Pend Proposal state. At this point the 45 day Reminder timer will be updated and the case
        /// will be sent through to the 45 Day Reminder state. The case is then returned to the Pend Proposal at which point the timer is recreated. The test will then
        /// update the timer to fire again, ensuring that the case does not move to the 45 Day Reminder state a second time.
        /// </summary>
        [Test, Description(@"This test will create a case and send the case through to the Pend Proposal state. At this point the 45 day Reminder timer will be updated
				and the case will be sent through to the 45 Day Reminder state. The case is then returned to the Pend Proposal at which point the timer is recreated. The test will
				then update the timer to fire again, ensuring that the case does not move to the 45 Day Reminder state a second time.")]
        public void FortyFiveTimerShouldOnlyFireOnceOnADebtCounsellingCase()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._45Days, 45, true);
            //perform the activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire45DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._45Days, base.TestCase.InstanceID, 1);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling._45DaysLapsed, base.TestCase.InstanceID, 1);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF._45DayReminder);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_45DayReminder);
            //load the case again
            base.LoadCase(WorkflowStates.DebtCounsellingWF._45DayReminder);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling._45dayremindersent);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case is back at Pend Proposal
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendProposal);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._45Days, 45, true);
            //perform the activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire45DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.DebtCounselling._45Days, base.TestCase.InstanceID, 2);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.DebtCounselling._45DaysCancel, base.TestCase.InstanceID, 1);
            //case shouldnt have moved
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendProposal);
        }
		
        #endregion Tests
    }
}