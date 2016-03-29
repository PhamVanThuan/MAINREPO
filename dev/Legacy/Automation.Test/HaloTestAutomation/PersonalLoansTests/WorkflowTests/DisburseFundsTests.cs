using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class DisburseFundsTests : PersonalLoansWorkflowTestBase<PersonalLoanDisbursement>
    {
        private bool IsAfterTwelveThirty { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            this.IsAfterTwelveThirty = Service<ICommonService>().IsTimeOver(new TimeSpan(12, 30, 00));
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //if (Service<ICommonService>().IsTimeOver(new TimeSpan(12, 30, 00)))
            //base.FailTest("This Disbursement Test can only run before the cut off time");
        }

        /// <summary>
        /// This test will complete the Disburse Funds action ensuring that the account is correctly opened and that the case moves to the following workflow
        /// state correctly. It will also check that an open account for the personal loan exists, that the disbursement records have been correctly setup and that
        /// the relevant transactions created.
        /// </summary>
        [Test]
        public void when_disbursing_a_personal_loan_application_a_personal_loan_account_is_created()
        {
            if (IsAfterTwelveThirty)
                base.FailTest("This Disbursement Test can only run before the cut off time");
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Disbursement, Common.Enums.WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            var supervisor = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLSupervisorD, RoundRobinPointerEnum.PLSupervisor);
            base.View.ClickConfirm();
            //we should have an open account
            var account = base.Service<IAccountService>().GetAccountByKey(base.personalLoanApplication.ReservedAccountKey);
            Assert.That(account.ProductKey == ProductEnum.PersonalLoan && account.AccountStatusKey == AccountStatusEnum.Open);
            //we should now have disbursement records
            DisbursementAssertions.AssertDisbursementAmount(base.personalLoanApplication.ReservedAccountKey, DisbursementStatusEnum.ReadyForDisbursement,
                DisbursementTransactionTypeEnum.Payment_NoInterest, Convert.ToDecimal(base.personalLoanApplication.LoanAmount));
            //we should have txn's posted into the account
            TransactionAssertions.AssertLoanTransactionExists(account.AccountKey, TransactionTypeEnum.PersonalLoan);
            TransactionAssertions.AssertLoanTransactionExists(account.AccountKey, TransactionTypeEnum.PersonalLoanInitiationFee);
            //we should have account roles for the legal entity
            var clientRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            AccountAssertions.AssertAccountRoleExists(base.personalLoanApplication.ReservedAccountKey, clientRole.LegalEntityKey);
            //we should have a mailing address
            var mailingAddress = Service<IAccountService>().GetMailingAddress(account.AccountKey);
            Assert.That(mailingAddress != null, string.Format("Account {0} has no mailing address set up", account.AccountKey));
            //we should have a financial service bank account
            var debitOrder = Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(account.AccountKey);
            Assert.That(debitOrder != null, string.Format("Account {0} has no financial service bank account set up", account.AccountKey));
            //stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DisburseFunds);
            //timer setup
            X2Assertions.AssertScheduledActivityTimer(base.GenericKey.ToString(), ScheduledActivities.PersonalLoans.DisbursedTimer, 1, false);
            //case moves states
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursed);
            Assert.That(offerExists);
            //assigned correctly
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, supervisor,
                WorkflowStates.PersonalLoansWF.Disbursed, Workflows.PersonalLoans);
        }

        /// <summary>
        /// Checks that the disbursement cut off rule is running whens starting the Disburse Funds activity after 12:30. This test WILL NOT run if it
        /// is executed prior to 12:30.
        /// </summary>
        [Test]
        public void when_the_time_is_after_twelve_thirty_a_personal_loan_application_cannot_be_disbursed()
        {
            if (!IsAfterTwelveThirty)
                base.FailTest("This Disbursement Test can only run after the cut off time");
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Disbursement, Common.Enums.WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.Browser.Page<BasePageAssertions>().AssertNotification("Disbursements are not allowed after: 12:30:00");
        }

        /// <summary>
        /// This test picks up a case the Manage Lead state and processes it using the ScriptEngine to the disbursement state to ensure
        /// that the case has the credit life premium option selected. It will then complete the disbursement and check that the life account
        /// is correctly created.
        /// </summary>
        [Test]
        public void when_disbursing_a_personal_loan_account_that_has_a_SAHL_credit_protection_plan_a_life_account_is_created()
        {
            if (IsAfterTwelveThirty)
                base.FailTest("This Disbursement Test can only run before the cut off time");
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD);
            //we push one through to make 100% certain we have an application with the credit life premium
            var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "CalculateApplicationToDisbursement", base.GenericKey);
            if (results.LastActivitySucceeded())
            {
                base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
                base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
                base.View.ClickConfirm();
                //we should have a related life account setup
                int lifeAccountKey = LifeAssertions.AssertCreditProtectionPlanAccountExistsByRelatedAccount(base.personalLoanApplication.ReservedAccountKey);
                //we should have account roles for the legal entity
                var clientRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
                AccountAssertions.AssertAccountRoleExists(lifeAccountKey, clientRole.LegalEntityKey);
            }
        }

        /// <summary>
        /// Once the case has been disbursed a 24hr timer should be setup. Once the timer elapses we should send the client a disbursement letter
        /// and a SMS confirming the disbursement. The case is sent to the archive and the application status is updated to accepted.
        /// </summary>
        [Test]
        public void when_the_disbursement_timer_fires_an_sms_should_be_sent_to_the_client()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Disbursement, Common.Enums.WorkflowRoleTypeEnum.PLSupervisorD);
            //check preferred correspondence medium
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(base.GenericKey);
            if (offerMailingAddress.CorrespondenceMedium == CorrespondenceMedium.Post)
                Service<IApplicationService>().InsertOfferMailingAddress(base.GenericKey);
            //we push one through to make 100% certain we have an application with the credit life premium
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.View.ClickConfirm();
            //timer setup
            X2Assertions.AssertScheduledActivityTimer(base.GenericKey.ToString(), ScheduledActivities.PersonalLoans.DisbursedTimer, 1, false);
            scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.FireDisbursementTimer, base.GenericKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2ScheduledActivity(ScheduledActivities.PersonalLoans.DisbursedTimer, base.InstanceID);
            //check that the SMS is sent
            string smsBody = string.Format(@"Your SA Home Loans Personal Loan for R {0} has been transferred into your debit order account.", base.personalLoanApplication.LoanAmount.ToString());
            //get roles
            var clientRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            //get legalEntity
            var legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: clientRole.LegalEntityKey);
            //wait for activity
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.PersonalLoans.EmailDisbursedLetter, base.InstanceID, 1);
            //case in the archive
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveDisbursed);
            Assert.That(offerExists);
            //stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DisbursedTimer);
            //offer status set to ACCEPTED
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.Accepted);
            //SMS added
            ClientEmailAssertions.AssertClientEmailSMS(smsBody, legalEntity.CellPhoneNumber, base.GenericKey);
        }

        [Test]
        public void when_an_external_life_policy_is_not_ceded_the_personal_loan_cannot_be_disbursed()
        {
            base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, false, false);
            base.scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.CalculateApplicationToDisbursementWithNoLifeCover, base.GenericKey);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
            var externalLife = base.CreateDefaultExternalCreditLife();
            externalLife.PolicyCeded = false;
            Service<IPersonalLoanService>().AddExternalLife(base.GenericKey, externalLife);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.Browser.Page<BasePageAssertions>().AssertNotification("External Policy is not ceded");
        }

        [Test]
        public void when_no_life_policy_exists_it_is_not_possible_to_disburse_the_personal_loan()
        {
            base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, false, false);
            base.scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.CalculateApplicationToDisbursementWithNoLifeCover, base.GenericKey);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.Browser.Page<BasePageAssertions>().AssertNotification("No Life Policy exists. Please capture an SAHL or External Life Policy");
        }

        [Test]
        public void when_an_external_life_policy_is_ceded_it_is_possible_to_disburse_the_personal_loan()
        {
            base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, false, false);
            base.scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.CalculateApplicationToDisbursementWithNoLifeCover, base.GenericKey);
            base.CreateDefaultExternalCreditLife();
            Service<IPersonalLoanService>().AddExternalLife(base.GenericKey, base.ExternalLife);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.View.ClickConfirm();
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursed);
            Assert.That(offerExists);
            LifeAssertions.AssertAccountExternalLifeCreated(base.personalLoanApplication.ReservedAccountKey);
        }

        [Test]
        public void when_a_personal_loan_with_ceded_external_life_policy_is_disbursed_the_life_policy_details_can_be_viewed_on_the_summary_screen()
        {
            base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, false, false);
            base.scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.CalculateApplicationToDisbursementWithNoLifeCover, base.GenericKey);
            base.CreateDefaultExternalCreditLife();
            Service<IPersonalLoanService>().AddExternalLife(base.GenericKey, base.ExternalLife);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.View.ClickConfirm();
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(Common.Constants.TestUsers.HaloUser);
            base.Browser.Navigate<LoanServicingCBO>().ExternalLife();
            base.Browser.Page<PersonalLoanExternalLifePolicySummary>().AssertFieldValues(this.ExternalLife);
        }

        [Test]
        public void when_a_personal_loan_with_SAHL_life_is_disbursed_all_life_policy_details_can_be_viewed_on_the_summary_screen()
        {
            base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, false, false);
            base.scriptEngine.ExecuteScript(WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.CalculateApplicationToDisbursementWithNoLifeCover, base.GenericKey);
            Service<IPersonalLoanService>().AddSAHLLife(base.GenericKey);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Disbursement, WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DisburseFunds);
            base.View.ClickConfirm();
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(Common.Constants.TestUsers.HaloUser);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(base.PersonalLoanAccount.LifeAccountKey);
            var lifePolicy = Service<IAccountService>().GetAccountByKey(base.PersonalLoanAccount.LifeAccountKey);
            var personalLoanAccountKey = Service<IApplicationService>().GetOfferAccountKey(base.GenericKey, false);
            double personalLoanSumInsured = Service<IAccountService>().GetCurrentPersonalLoanBalance(personalLoanAccountKey);
            base.Browser.Page<CreditProtectionSummary>().AssertFieldValues(lifePolicy, personalLoanSumInsured);
        }
    }
}