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
    /// Contains tests for the cancellation section of the debt counselling workflow
    /// </summary>
    [RequiresSTA]
    public sealed class CancellationTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test performs the Debt Counselling Cancelled action. It ensures that a debt counselling case can be cancelled by the debt counselling consultant.
        /// The test will check that the cases status is set to cancelled, the selected reason is added to the case, the required stage transition is written and
        /// the case is archived.
        /// </summary>
        [Test, Description(@"This test performs the Debt Counselling Cancelled action. It ensures that a debt counselling case can be cancelled by the debt counselling
		consultant. The test will check that the cases status is set to cancelled, the selected reason is added to the case, the required stage transition is written and
		the case is archived")]
        public void CancelDebtCounsellingPriorToAcceptance()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //cancel the debt counselling case
            CancelDebtCounselling(ReasonDescription.CaseCreatedInError);
        }

        /// <summary>
        /// This test performs the Debt Counselling Cancelled action without selecting a reason. It ensures that a validation message is displayed ensuring that
        /// a cancellation message is selected.
        /// </summary>
        [Test, Description(@"This test performs the Debt Counselling Cancelled action without selecting a reason. It ensures that a validation message is displayed
		ensuring that a cancellation message is selected.")]
        public void CancelDebtCounsellingNoReason()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CancelDebtCounselling);
            //cancel the debt counselling case without selecting a reason
            base.Browser.Page<DebtCounsellingCancelled>().CancelDebtCounsellingSubmitNoReason();
            //check that a validation messages exists instructing the user to select a reason
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a reason");
        }

        /// <summary>
        /// This test performs the Debt Counselling Cancelled action on a case linked to other open debt counselling cases. It ensures that the accounts for all the
        /// linked debt counselling cases are displayed on the cancellation screen.
        /// </summary>
        [Test, Description(@"This test performs the Debt Counselling Cancelled action on a case linked to other open debt counselling cases. It ensures that the
		accounts for all the linked debt counselling cases are displayed on the cancellation screen.")]
        public void CancelDebtCounsellingGroupedAccounts()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, false, 2, false, searchForCase: false);
            //cancel the debt counselling case
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CancelDebtCounselling);
            //check the grouped accounts are displayed on the cancellation screen
            base.Browser.Page<DebtCounsellingCancelled>().AssertGroupedAccountsExistOnCancellation(base.TestCase.DebtCounsellingKey);
        }

        /// <summary>
        /// The test will perform the Debt Counselling Cancelled action on a case with an existing active proposal. It ensures that the case is cancelled, the
        /// debt counselling rate overrides are deactivated, the suspended debit order detail type is removed and the stop debt counselling memo transaction
        /// is posted.
        /// </summary>
        [Test, Description(@"The test will perform the Debt Counselling Cancelled action on a case with an existing active proposal. It ensures that the case
		is cancelled, the debt counselling rate overrides are deactivated and the stop debt counselling memo transaction is posted.")]
        public void CancelDebtCounsellingPostAcceptance()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.AcceptedProposal, TestUsers.DebtCounsellingConsultant);
            //fetch current active debt counselling rate overrides
            List<FinancialAdjustmentTypeSourceEnum> finAdjustments =
                base.Service<IFinancialAdjustmentService>().GetAccountFinancialAdjustmentsByStatus(FinancialAdjustmentStatusEnum.Active,
                base.TestCase.AccountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment,
                FinancialAdjustmentTypeSourceEnum.DebtCounseling_PaymentAdjustment, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_15,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_16, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_17,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_18, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_22,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_23, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_24,
                FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_25, FinancialAdjustmentTypeSourceEnum.DebtCounseling_PaymentAdjustment_9);
            //cancel the debt counselling case
            CancelDebtCounselling(ReasonDescription.ClientHasVoluntarilyCancelled);
            //check if each of the active rate overrides have been set to inactive
            foreach (FinancialAdjustmentTypeSourceEnum fin in finAdjustments)
            {
                FinancialAdjustmentAssertions.AssertFinancialAdjustment(base.TestCase.AccountKey, fin, FinancialAdjustmentStatusEnum.Active, false);
            }
            //check the stop debt counselling memo transaction has been posted
            TransactionAssertions.AssertLoanTransactionExists(base.TestCase.AccountKey, TransactionTypeEnum.DebtCounselllingOptOut, -2);
        }

        /// <summary>
        /// This test performs the Debt Counselling Cancelled action. It ensures that a debt counselling case can be cancelled by the debt counselling
        /// consultant. The test will check that the cases status is set to cancelled, the selected reason is added to the case, the required stage
        /// transition is written and the case is archived.
        /// </summary>
        [Test, Description(@"This test performs the Debt Counselling Cancelled action. It ensures that a debt counselling case can be cancelled by the debt counselling
		consultant. The test will check that the cases status is set to cancelled, the selected reason is added to the case, the required stage transition is written and
		the case is archived")]
        public void CancelDebtCounsellingWithDeathNotification()
        {
            //search for account with ework case at debt counselling stage in loss control
            var Account = Service<IDebtCounsellingService>().GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase();
            string id = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(Account.AccountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, true, 1, true, product: Account.ProductKey, searchForCase: false, idNumber: id);
            string eFolderID = Service<IEWorkService>().GeteFolderIdForCaseInLossControl(Account.AccountKey);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.NotifiedofDeath);
            //we need all of the LE's on our account
            var leKeys = base.Service<IAccountService>().AccountRoleLegalEntityKeys(Account.AccountKey);
            WorkflowHelper.NotificationOfDeathOrSequestrationMultipleLegalEntities(base.Browser, leKeys, base.TestCase.InstanceID, base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, NotificationTypeEnum.Death,
                ReasonDescription.NotificationofDeath, Common.Enums.CorrespondenceTemplateEnum.DeceasedNotificationNoLiving, Account.AccountKey);
            //cancel the debt counselling case
            CancelDebtCounselling(ReasonDescription.DCCancelledClientDeceased);
            //check the ework case has been moved to the assign attorney stage in the loss control map
            Service<IEWorkService>().WaitForEworkEvent(eFolderID, EworkActions.X2ReturnDebtCounselling, DateTime.Now.AddMinutes(-2).ToString(Formats.DateTimeFormatSQL), 30);
            eWorkAssertions.AssertEworkCaseExists(Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl);
        }

        /// <summary>
        /// If the existing eWork case is not in the Debt Counselling stage, leave it there. Do nothing.
        /// if there is an existing ework case at the Debt Counselling Collections, Debt Counselling Arrears or Debt Counselling Litigation stages)
        /// move it back to the stage it was in prior to it moving into the Debt Counselling stage.
        /// </summary>
        [Test, Description(@"If the existing eWork case is not in the Debt Counselling stage, leave it there. Do nothing.
        if there is an existing ework case at the Debt Counselling Collections, Debt Counselling Arrears or Debt Counselling Litigation stages)
        move it back to the stage it was in prior to it moving into the Debt Counselling stage.")]
        public void CancelWithCaseCreatedInError()
        {
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(false, new List<string> { EworkStages.AllOver });
            CancelDebtCounselling(ReasonDescription.CaseCreatedInError);
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), eWorkCase.BackToStage, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), eWorkCase.BackToStage);
        }

        /// <summary>
        /// If it is between 1.1 and 2 months in arrears then move existing eWork case to Arrears Map (Arrear Cases stage). The case will need to be round robin assigned
        /// </summary>
        [Test, Description(@"If it is between 1.1 and 2 months in arrears then move existing eWork case to Arrears Map (Arrear Cases stage). The case will need to be round robin assigned ")]
        public void CancelCaseBetween1and2MonthsInArrears()
        {
            //Have to do this before cancelling, else the token will move
            var expectedAssignedUser = Service<IEWorkService>().GetNextUserForRoundRobinAssignmentInLossControl("LC Arrears Non-Subsidy");
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(false, new List<string> { EworkStages.AllOver });
            base.Service<IAccountService>().UpdateLoanMonthsInArrears(accountkey: eWorkCase.Account.AccountKey, monthsInArrears: 1.5M);
            CancelDebtCounselling(ReasonDescription.DCCancelledDuetoNonPayment);
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), EworkStages.ArrearCases, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), EworkStages.ArrearCases, EworkMaps.LossControl);
            eWorkAssertions.AssertLossControlUserToDo(eWorkCase.Account.AccountKey.ToString(), EworkMaps.LossControl, EworkStages.ArrearCases, expectedAssignedUser.eUserName);
        }

        /// <summary>
        /// If it is less than 1.1 months in arrears then move existing eWork case to Collections Map (Collections stage). The case will need to be round robin assigned.
        /// </summary>
        [Test, Description(@"If it is less than 1.1 months in arrears then move existing eWork case to Collections Map (Collections stage). The case will need to be round robin assigned.")]
        public void CancelCaseLessThan1MonthInArrears()
        {
            //Have to do this before cancelling, else the token will move
            var expectedAssignedUser = Service<IEWorkService>().GetNextUserForRoundRobinAssignmentInLossControl("LC Collections Subsidy");
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(true, new List<string> { EworkStages.AllOver });
            base.Service<IAccountService>().UpdateLoanMonthsInArrears(accountkey: eWorkCase.Account.AccountKey, monthsInArrears: 0.5M);
            CancelDebtCounselling(ReasonDescription.DCCancelledDuetoNonPayment);
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), EworkStages.Collections, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), EworkStages.Collections, EworkMaps.LossControl);
            eWorkAssertions.AssertLossControlUserToDo(eWorkCase.Account.AccountKey.ToString(), EworkMaps.LossControl, EworkStages.Collections, expectedAssignedUser.eUserName);
        }

        /// <summary>
        /// if it is greater than 2 months in arrears then move existing eWork case to Litigation Map (Assign Attorney stage)
        /// </summary>
        [Test, Description(@"if it is greater than 2 months in arrears then move existing eWork case to Litigation Map (Assign Attorney stage) ")]
        public void CancelCaseGreaterThan2MonthsInArrears()
        {
            //Have to do this before cancelling, else the token will move
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(false, new List<string> { EworkStages.AllOver });
            base.Service<IAccountService>().UpdateLoanMonthsInArrears(accountkey: base.TestCase.AccountKey, monthsInArrears: 2.1M);
            CancelDebtCounselling(ReasonDescription.DCCancelledDuetoNonPayment);
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl);
        }

        /// <summary>
        /// When a case is archived in X2 and the reason was DC cancelled client sequestrated, if there is already an active case in eWork. Do nothing to it.
        /// </summary>
        [Test, Description(@"When a case is archived in X2 and the reason was DC cancelled client sequestrated, if there is already an active case in eWork. Do nothing to it.")]
        public void DebtCounsellingCancelledClientSequestrated()
        {
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(false, new List<string> { EworkStages.AllOver });
            var eworkstagebeforecancel = Service<IEWorkService>().GetEworkStage(eWorkCase.Account.AccountKey);
            CancelDebtCounselling(ReasonDescription.DCCancelledClientSequestrated);
            //Nothing should have changed!
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), eworkstagebeforecancel, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), eworkstagebeforecancel, EworkMaps.LossControl);
        }

        /// <summary>
        /// When a case is archived in X2 and the reason was DC cancelled client deceased then move existing eWork case to Litigation Map (Assign Attorney stage).
        /// </summary>
        [Test, Description(@"When a case is archived in X2 and the reason was DC cancelled client deceased then move existing eWork case to Litigation Map (Assign Attorney stage).")]
        public void DebtCounsellingCancelledClientDeceased()
        {
            var eWorkCase = this.CreateCancelTestCaseFromEworkAtPendProposal(false, new List<string> { EworkStages.AllOver });
            CancelDebtCounselling(ReasonDescription.DCCancelledClientDeceased);
            Service<IEWorkService>().WaitForEWorkStage(eWorkCase.Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl, 3);
            eWorkAssertions.AssertEworkCaseExists(eWorkCase.Account.AccountKey.ToString(), EworkStages.AssignAttorney, EworkMaps.LossControl);
        }

        #region Helpers

        /// <summary>
        /// Gets an account from ework and create a debtcounselling case from it at pend proposal
        /// </summary>
        /// <param name="isSubsidy"></param>
        /// <param name="eWorkStage"></param>
        /// <returns></returns>
        private Automation.DataModels.eWorkCase CreateCancelTestCaseFromEworkAtPendProposal(bool isSubsidy, List<string> eWorkStage)
        {
            var eworkLossControlCase = Service<IEWorkService>().GetEWorkCase(EworkMaps.LossControl, eWorkStage, IsSubsidised: isSubsidy);
            Assert.That(eworkLossControlCase != null, string.Format(@"No e-Work Loss Control case was found: eWorkStage: {0}", eWorkStage));

            var accounts = WorkflowHelper.CreateCaseAndSendToState(WorkflowStates.DebtCounsellingWF.PendProposal, true, 1, false, idNumber: eworkLossControlCase.IDNumber);
            base.TestCase = accounts[0];
            //reload ework case
            var stageList = new List<string> {
                                                EworkStages.DebtCounselling,
                                                EworkStages.DebtCounselling_Arrears,
                                                EworkStages.DebtCounselling_Collections,
                                                EworkStages.DebtCounselling_Estates,
                                                EworkStages.DebtCounselling_Facilitation,
                                                EworkStages.DebtCounselling_Seq
                                            };
            eworkLossControlCase = Service<IEWorkService>().GetEWorkCase(EworkMaps.LossControl, stageList, accountkey: base.TestCase.AccountKey);
            base.TestCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases(debtcounsellingkey: base.TestCase.DebtCounsellingKey).FirstOrDefault();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.PendProposal);

            return new Automation.DataModels.eWorkCase(eworkLossControlCase, base.TestCase);
        }

        #endregion Helpers

        private void CancelDebtCounselling(string reasonDescription)
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.CancelDebtCounselling);
            base.Browser.Page<DebtCounsellingCancelled>().CancelDebtCounselling(reasonDescription);
            //check that the status of the debt counselling case is set to Cancelled
            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Cancelled, base.TestCase.DebtCounsellingKey);
            //check that the debt counselling case has been archived
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ArchiveDebtCounselling);
            //check that the correct reason exists for the debt counselling case
            ReasonAssertions.AssertReason(reasonDescription, Common.Constants.ReasonType.DebtCounsellingCancelled, base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey);
            //check that the Debt Review Cancelled stage transition was written
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DebtReviewCancelled);
        }
    }
}