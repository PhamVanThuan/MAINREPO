using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing.CATSDisbursement;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class CATSDisbursementTests : PersonalLoansWorkflowTestBase<CATSDisbursementAdd>
    {
        private TimeSpan cutOffTime = new TimeSpan(12, 30, 00);

        private Automation.DataModels.LoanFinancialService PersonalLoanFinancialService { get; set; }

        private bool isAfterTwelveThirty;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoansClientServiceManager);
            isAfterTwelveThirty = Service<ICommonService>().IsTimeOver(cutOffTime);
        }

        protected override void OnTestStart()
        {
            base.GenericKey = Helper.FindPersonalLoanAccount(true);
            base.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(this.GenericKey);
            this.PersonalLoanFinancialService = PersonalLoanAccount.FinancialServices.FirstOrDefault(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.PersonalLoan);
            base.SearchAndLoadAccount();
            base.Browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
        }

        [Test]
        public void _01_when_posting_a_refund_the_initial_balance_cannot_be_exceeded()
        {
            var amtToPost = (PersonalLoanFinancialService.InitialBalance - PersonalLoanAccount.CurrentBalance) + 1.00M;
            base.View.AddDisbursement(DisbursementTransactionTypes.Refund, amtToPost, "Should not post", 1, amtToPost, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("cannot be greater than the initial balance");
        }

        [Test]
        public void _02_when_posting_a_refund_only_the_debitorderbankaccount_can_be_used()
        {
            //we need to reduce the balance
            Service<ILoanTransactionService>().pProcessAccountPaymentTran(PersonalLoanAccount.AccountKey, TransactionTypeEnum.PrePayment320, 500.00, "Refund Disbursement Test",
                TestUsers.PersonalLoansClientServiceManager, DateTime.Now);
            var financialServiceBankAccounts = (from fsBankAcc in Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(PersonalLoanAccount.AccountKey)
                                                where fsBankAcc.GeneralStatusKey == (int)GeneralStatusEnum.Active
                                                select fsBankAcc).FirstOrDefault();
            var bankAccount = Service<IBankingDetailsService>().GetBankAccountByBankAccountKey(financialServiceBankAccounts.BankAccountKey.Value);
            //get the formatted string
            var bankAccountString = Service<IBankingDetailsService>().GetBankAccountStringList(new List<Automation.DataModels.BankAccount> { bankAccount });
            base.View.AssertBankAccountList(bankAccountString);
        }

        [Test]
        public void _03_when_posting_a_refund_the_time_must_be_prior_to_twelve_thirty()
        {
            if (!isAfterTwelveThirty)
                base.FailTest("This test can only run after the cut off time");
            //we need to reduce the balance
            Service<ILoanTransactionService>().pProcessAccountPaymentTran(PersonalLoanAccount.AccountKey, TransactionTypeEnum.PrePayment320, 500.00, "Refund Disbursement Test",
                TestUsers.PersonalLoansClientServiceManager, DateTime.Now);
            base.View.AddDisbursement(DisbursementTransactionTypes.Refund, 200.00M, "SHOULD NOT POST", 1, 200.00M, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Disbursements are not allowed after: 12:30:00");
        }

        [Test]
        public void _04_when_using_CATS_Disbursement_as_a_PLUser_only_the_refund_option_is_available()
        {
            var disbursementTypes = new List<string>() {
                    DisbursementTransactionTypes.Refund
                };
            base.View.AssertDisbursementTypeOptions(disbursementTypes);
        }

        [Test]
        public void _05_when_posting_a_refund_disbursement_and_transaction_records_should_be_posted()
        {
            if (isAfterTwelveThirty)
                base.FailTest("This Disbursement Test can only run before the cut off time");
            //we need to reduce the balance
            Service<ILoanTransactionService>().pProcessAccountPaymentTran(PersonalLoanAccount.AccountKey, TransactionTypeEnum.PrePayment320, 500.00, "Refund Disbursement Test",
                TestUsers.PersonalLoansClientServiceManager, DateTime.Now);
            PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(PersonalLoanAccount.AccountKey);
            var balancePriorToDisbursement = PersonalLoanAccount.CurrentBalance;
            //refund half it back
            var disbursementAmount = 250.00M;
            base.View.AddDisbursement(DisbursementTransactionTypes.Refund, disbursementAmount, "SHOULD POST", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.View.ClickButton(ButtonTypeEnum.Post);
            //should have a disbursement record
            DisbursementAssertions.AssertDisbursementExistsAtStatus(PersonalLoanAccount.AccountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.Refund);
            DisbursementAssertions.AssertDisbursementAmount(PersonalLoanAccount.AccountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.Refund, disbursementAmount);
            //should have a loan tran
            TransactionAssertions.AssertLoanTransactionExists(PersonalLoanAccount.AccountKey, TransactionTypeEnum.Refund);
            //balance should have increased
            PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(PersonalLoanAccount.AccountKey);
            Assert.That(balancePriorToDisbursement + disbursementAmount == PersonalLoanAccount.CurrentBalance);
        }

        [Test]
        public void _06_when_posting_a_refund_and_debit_order_bank_account_changed_should_display_new_bank_account_()
        {
            Service<IDebitOrdersService>().UpdateFinancialServiceBankAccountStatus(base.GenericKey, GeneralStatusEnum.Inactive);
            var legalEntity = base.Service<ILegalEntityService>().GetLegalEntityRoles(accountKey: base.GenericKey).FirstOrDefault();
            Service<ILegalEntityService>().InsertLegalEntityBankAccount(legalEntity.LegalEntityKey);
            Service<IDebitOrdersService>().InsertFirstLegalEntityBankAccountAsFSBankAccount(base.GenericKey, FinancialServicePaymentTypeEnum.DebitOrderPayment);

            var financialServiceBankAccounts = (from fsBankAcc in Service<IDebitOrdersService>().GetFinancialServiceBankAccounts(PersonalLoanAccount.AccountKey)
                                                where fsBankAcc.GeneralStatusKey == (int)GeneralStatusEnum.Active
                                                select fsBankAcc).FirstOrDefault();
            var bankAccount = Service<IBankingDetailsService>().GetBankAccountByBankAccountKey(financialServiceBankAccounts.BankAccountKey.Value);
            //get the formatted string
            var bankAccountString = Service<IBankingDetailsService>().GetBankAccountStringList(new List<Automation.DataModels.BankAccount> { bankAccount });
            base.Browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
            base.View.AssertBankAccountList(bankAccountString);
        }
    }
}