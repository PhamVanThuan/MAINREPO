using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    ///  Contains tests for the termination section of the debt counselling workflow
    /// </summary>
    [RequiresSTA]
    public sealed class TerminationTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test case will create a case and move it through to the Manage Proposal state. At this point the assigned user can perform the 'Terminate Application'
        /// action that will move the case through to the Termination state. The test ensures that the reason selected is added to the Reason table against the
        /// DebtCounsellingKey and that the workflow assignment is maintained.
        /// </summary>
        [Test, Description(@"This test case will create a case and move it through to the Manage Proposal state. At this point the assigned user can perform the 'Terminate Application'
		action that will move the case through to the Termination state. The test ensures that the reason selected is added to the Reason table against the
		DebtCounsellingKey and that the workflow assignment is maintained.")]
        public void TerminateApplicationFromManageProposal()
        {
            //search for a case at ManageProposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);
            //we can now perform the terminate application
            base.TestCase.AssignedUser = adUserName;
            TerminateApplication();
        }

        /// <summary>
        /// The termination reason is used on a subsequent report so the user is required to select only one reason before the action can be completed. This test will ensure
        /// that you cannot continue if no reason is selected or if multiple reasons are selected.
        /// </summary>
        [Test, Description(@"The termination reason is used on a subsequent report so the user is required to select only one reason before the action can be completed.
		This test will ensure that you cannot continue if no reason is selected or if multiple reasons are selected.")]
        public void TerminateApplicationOnlyOneReasonRequired()
        {
            //search for a case at ManageProposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we can now perform the terminate application
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.TerminateApplication);
            //submit without any reasons
            base.Browser.Page<CommonReasonCommonDecline>().SubmitWithoutReason();
            //should still be on the reason screen
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Update Reasons");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Must select at least one reason");
            List<string> selectedReasons = base.Browser.Page<CommonReasonCommonDecline>().SelectMultipleReasons(ReasonType.DebtCounsellingTermination, 2);
            //submit with multiple reasons;
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Update Reasons");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Only one reason can be selected");
            //case hasnt moved
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
        }

        /// <summary>
        /// This test will terminate the debt counselling case after it has been accepted in order to ensure that the debt counselling financial adjustments
        /// are correctly deactivated.
        /// </summary>
        [Test, Description(@"This test will terminate the debt counselling case after it has been accepted in order to ensure that the debt counselling financial adjustments
		are correctly deactivated.")]
        public void TenDayTerminationTimerPostAcceptance()
        {
            //search for a case at Pend Payment
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            //remove court details so that the case archives
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            TerminateApplication();
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendTerminationLetter, base.TestCase.DebtCounsellingKey);
            //we need to get the current active debt counselling rate overrides
            var finAdjustments = base.Service<IFinancialAdjustmentService>().GetAccountFinancialAdjustmentsByStatus(FinancialAdjustmentStatusEnum.Active,
                base.TestCase.AccountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment,
                FinancialAdjustmentTypeSourceEnum.DebtCounseling_PaymentAdjustment, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_15,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_16, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_17,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_18, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_22,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_23, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_24,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_25, FinancialAdjustmentTypeSourceEnum.DebtCounseling_PaymentAdjustment_9);
            //fire the timer
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire10DaysTimer, base.TestCase.DebtCounsellingKey);
            //wait for the case to reach the archive
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.TenDayTimer, base.TestCase.InstanceID, 1);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.TerminationArchive);
            //check if each of the active rate overrides have been set to inactive
            foreach (FinancialAdjustmentTypeSourceEnum fin in finAdjustments)
            {
                FinancialAdjustmentAssertions.AssertFinancialAdjustment(base.TestCase.AccountKey, fin, FinancialAdjustmentStatusEnum.Canceled, true);
            }
        }

        /// <summary>
        /// This test case will terminate an application from Manage Proposal in order to ensure that we do not try and run the opt out proc on cases
        /// that have not yet had their proposals accepted.
        /// </summary>
        [Test, Description(@"This test case will terminate an application from Manage Proposal in order to ensure that we do not try and run the opt out proc on cases
		that have not yet had their proposals accepted.")]
        public void TenDayTerminationTimerPriorToAcceptance()
        {
            //search for a case at Pend Payment
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            TerminateApplication();
            SendTerminationLetterToDebtCounsellorAndClientsWithDifferentDomiciliums();
            //update the scheduled activity
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire10DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.TenDayTimer, base.TestCase.InstanceID, 1);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.TerminationArchive);
            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Terminated, base.TestCase.DebtCounsellingKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DebtCounsellingTerminated);
        }

        /// <summary>
        /// If court details have been captured against a debt counselling case then the case should not be moved into the archive when the 10 Day Timer
        /// is fired. It should remain at the Termination Letter Sent state.
        /// </summary>
        [Test, Sequential, Description(@"If court details have been captured against a debt counselling case then the case should not be moved into the archive when the 10 Day Timer
		is fired. It should remain at the Termination Letter Sent state.")]
        public void TenDayTerminationCourtDetailsExist([Values(HearingTypeEnum.Court, HearingTypeEnum.Tribunal, HearingTypeEnum.Court, HearingTypeEnum.Tribunal)]
														HearingTypeEnum hearingType,
                                                       [Values(HearingAppearanceTypeEnum.CourtCourtApplication, HearingAppearanceTypeEnum.TribunalCourtApplication,
                                                       HearingAppearanceTypeEnum.CourtOrderGranted, HearingAppearanceTypeEnum.TribunalOrderGranted)]
														HearingAppearanceTypeEnum appearanceType)
        {
            //search for a case at Termination Letter Sent
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //we need to remove any existing court details
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            //insert new ones
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, hearingType, appearanceType, DateTime.Now.AddDays(5));
            //update the scheduled activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, base.TestCase.DebtCounsellingKey);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendTerminationLetter, base.TestCase.DebtCounsellingKey);

            try
            {
                var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire10DaysTimer, base.TestCase.DebtCounsellingKey);
            }
            catch (Exception ex)
            {
                Assert.That(ex.ToString().Contains("Court Hearing Appearance Types Exist."), "OnStart() failed for an unexpected Reason");
            }
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.TerminationLetterSent);
        }

        /// <summary>
        /// The test case will find an ework Loss Control case that does not have a corresponding ework case and use it to create a case.
        /// Once created the case will be terminated, this should results in the ework case moving into the Assign Attorney stagein the Loss Control
        /// Ework map.
        /// </summary>
        [Test, Description(@"The test case will find an ework Loss Control case that does not have a corresponding debt counselling case and use it to create a case.
		Once created the case will be terminated, this should results in the ework case moving into the Assign Attorney stage in the Loss Control
		Ework map.")]
        public void TerminateDebtCounsellingWithEworkCase()
        {
            //search for account with ework case at debt counselling stage in loss control
            var Account = Service<IDebtCounsellingService>().GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase();
            string id = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(Account.AccountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.TestCase = WorkflowHelper.CreateCaseAndSendToState(WorkflowStates.DebtCounsellingWF.TerminationLetterSent, idNumber: id).FirstOrDefault();
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire10DaysTimer, base.TestCase.DebtCounsellingKey);
            //wait for archive to complete
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.TenDayTimer, base.TestCase.InstanceID, 1);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.TerminationArchive);
            //check the ework case has been moved to the assign attorney stage in the loss control map
            eWorkAssertions.AssertEworkCaseExists(Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl);
        }

        /// <summary>
        /// This test will ensure that if a case has been through Payment in Order and a 972 transaction exists, the Terminate Application action will post a 973 transaction
        /// in order to offset the 972 transaction.
        /// </summary>
        [Test, Description(@"This test will ensure that if a case has been through Payment in Order and a 972 transaction exists, the Terminate Application action will post a 973 transaction
        in order to offset the 972 transaction.")]
        public void TerminateApplicationPostsDebtReviewArrangementDebit()
        {
            //search for a case at Payment Review
            base.StartTest(WorkflowStates.DebtCounsellingWF.PaymentReview, TestUsers.DebtCounsellingSupervisor);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.PaymentInOrder, base.TestCase.DebtCounsellingKey);
            //wait until case is in debt review approved
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.DebtReviewApproved);
            //fire the flag to default in payment
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EXTIntoArrears, base.TestCase.DebtCounsellingKey);
            //remove court details so that the case archives
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            //terminate from Default in Payment, reload the case first to get the assigned consultant.
            base.ReloadTestCase();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DefaultinPayment);
            TerminateApplication();
            //assert the expected transaction exists
            TransactionAssertions.AssertArrearTransactionExists(base.TestCase.AccountKey, TransactionTypeEnum.DebtReviewArrangementDebit, -2);
        }

        /// <summary>
        /// This test will ensure that if a case has been through Payment in Order and a 972 transaction exists, the 5 Days timer will post a 973 transaction
        /// in order to offset the 972 transaction.
        /// </summary>
        [Test, Description(@"This test will ensure that if a case has been through Payment in Order and a 972 transaction exists, the 5 Days timer will post a 973 transaction
        in order to offset the 972 transaction.")]
        public void FiveDayTimerPostsDebtReviewArrangementDebit()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PaymentReview, TestUsers.DebtCounsellingSupervisor, searchForCase: false);
            //running the scripts to move the case
            scriptEngine.ExecuteScript(WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.PaymentInOrder, base.TestCase.DebtCounsellingKey);
            scriptEngine.ExecuteScript(WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.ChangeInCircumstance, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.ManageProposal);
            //fire the 60 day flag
            scriptEngine.ExecuteScript(WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RaiseEXT60DateNoDateOrPayment, base.TestCase.DebtCounsellingKey);
            //fire 5 day timer
            scriptEngine.ExecuteScript(WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.Fire5DaysTimer, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.Termination);
            //assert the expected transaction exists
            TransactionAssertions.AssertArrearTransactionExists(base.TestCase.AccountKey, TransactionTypeEnum.DebtReviewArrangementDebit, -2);
        }

        /// <summary>
        /// Tests that the Intention to Terminate document can be sent via the workflow action.
        /// </summary>
        [Test, Description("Tests that the Intention to Terminate document can be sent via the workflow action.")]
        public void SendIntentToTerminateLetter()
        {
            //search for a case at Termination Letter Sent
            base.StartTest(WorkflowStates.DebtCounsellingWF.IntenttoTerminate, TestUsers.DebtCounsellingConsultant);
            int legalEntityKey = Service<IDebtCounsellingService>().GetDCTestCaseDebtCounsellorCorrespondenceDetails(null, base.TestCase.AccountKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendIntenttoTerminate);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectCorrespondenceRecipient(legalEntityKey);
            //check the document has been sent
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.IntentionToTerminate, CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.IntentionToTerminate, CorrespondenceMedium.Post, base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
            //check transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_IntentToTerminate);
        }

        private void TerminateApplication()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.TerminateApplication);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.DebtCounsellingTermination);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.DebtCounsellingTermination, base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.Termination);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.Termination);
            //the case should be assigned to our consultant role still
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.Termination);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, base.TestCase.AssignedUser);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
        }

        private void SendTerminationLetterToDebtCounsellorAndClientsWithDifferentDomiciliums()
        {
            var externalRoleList = Service<IExternalRoleService>().GetActiveExternalRoleList(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.Client);
            foreach (var client in externalRoleList)
            {
                Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(client.LegalEntityKey);
                //insert legalentityaddress
                var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(client.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
                Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, client.LegalEntityKey, GeneralStatusEnum.Active);
            }
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendTerminationLetter);
            var debtCounsellorRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.DebtCounsellor);
            externalRoleList.Add(debtCounsellorRole);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectMultipleCorrespondenceRecipientsForPost(externalRoleList);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().ClickSendCorrespondence();
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.SendTerminationLetter, base.TestCase.InstanceID, 1);
            //the case should be assigned to our consultant role still
            DebtCounsellingAssertions.AssertX2StateByAccountKey(base.TestCase.AccountKey, WorkflowStates.DebtCounsellingWF.TerminationLetterSent);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, base.TestCase.AssignedUser);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
            //scheduled activity should be set up
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling.Tendays, 10, true);
            var legalEntityKeys = (from er in externalRoleList select er.LegalEntityKey).ToList();
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingTerminationLetter, CorrespondenceMedium.Post, base.TestCase.AccountKey, legalEntityKeys, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_TerminationLetterSent);
        }
    }
}