using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class ChangeInstalmentTests : PersonalLoansWorkflowTestBase<PersonalLoanChangeInstalment>
    {
        private Automation.DataModels.LoanFinancialService PersonalLoanFinancialService { get; set; }

        private Automation.DataModels.Account CreditLifePolicy { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingManager);
        }

        protected override void OnTestStart()
        {
            base.GenericKey = Helper.FindPersonalLoanAccount(true);
            GetAccountDetails();
            var results = Service<IAccountService>().GetOpenRelatedAccountsByProductKey(PersonalLoanAccount.AccountKey, Common.Enums.ProductEnum.SAHLCreditProtectionPlan);
            CreditLifePolicy = Service<IAccountService>().GetAccountByKey(results.Rows(0).Column("AccountKey").GetValueAs<int>());
            base.SearchAndLoadAccount();
            base.Browser.Navigate<LoanServicingCBO>().ChangeInstalment();
        }

        private void GetAccountDetails()
        {
            base.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(this.GenericKey);
            this.PersonalLoanFinancialService = PersonalLoanAccount.FinancialServices.FirstOrDefault(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.PersonalLoan);
        }

        [Test, Description(@"The test will post a pre payment to reduce the current balance of the personal loan. It will then calculate the new instalment on the account and process the term change
                            ensuring that the new instalment matches what we expect, the transactions are posted, the memo populated with the comment and the correct stage transition is written.")]
        public void ChangePersonalInstalmentUpdatesInstalment()
        {
            //post a pre payment to reduce balance
            Service<ILoanTransactionService>().pProcessTran(this.PersonalLoanFinancialService.FinancialServiceKey, Common.Enums.TransactionTypeEnum.PrePayment320, 1000.00M, "Change Instalment Test", TestUsers.PersonalLoansClientServiceManager);
            GetAccountDetails();
            //calculate the new instalment
            var calculatedInstalment = Service<ICalculationsService>().CalculateLoanInstalment(Convert.ToDouble(PersonalLoanAccount.CurrentBalance), Convert.ToDouble(PersonalLoanFinancialService.InterestRate), PersonalLoanFinancialService.RemainingInstalments);
            string comment = "Change Instalment Test";
            base.View.ChangeInstalment(comment);
            GetAccountDetails();
            var newInstalment = this.PersonalLoanFinancialService.Payment;
            Assert.That(newInstalment == calculatedInstalment, string.Format(@"The new instalment of {0} did not match the expected instalment of {1} for personal loan account {2}", newInstalment, calculatedInstalment, base.GenericKey));
            //should have loan transactions for instalment change and the term change
            TransactionAssertions.AssertLoanTransactionExists(base.PersonalLoanAccount.AccountKey, FinancialServiceTypeEnum.PersonalLoan, Common.Enums.TransactionTypeEnum.InstallmentChange);
            //should have a memo record
            MemoAssertions.AssertLatestMemoInformation(PersonalLoanAccount.AccountKey, GenericKeyTypeEnum.Account_AccountKey, MemoTable.Memo, string.Format(@"Instalment Change: {0}", comment));
            //should have a stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(PersonalLoanAccount.AccountKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ChangeInstalment);
        }

        [Test, Description("A instalment change cannot be processed if a comment is not provided.")]
        public void ChangePersonalLoanInstalmentWithoutCommentDisplaysWarning()
        {
            base.View.PopulateComment(string.Empty);
            base.View.ClickCalculate();
            base.View.ClickChangeInstalment();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Instalment Change Request, please add Comment.");
        }

        [Test, Description("Checks that the instalment breakdown is correctly populated with the new instalment, service fee and life premium values.")]
        public void CheckInstalmentBreakdown()
        {
            //post a pre payment to reduce balance
            Service<ILoanTransactionService>().pProcessTran(this.PersonalLoanFinancialService.FinancialServiceKey, Common.Enums.TransactionTypeEnum.PrePayment320, 1000.00M, "Change Instalment Test", TestUsers.PersonalLoansClientServiceManager);
            GetAccountDetails();
            base.View.PopulateComment("Instalment Change Test");
            base.View.ClickCalculate();
            var loanInstalment = Service<ICalculationsService>().CalculateLoanInstalment(Convert.ToDouble(PersonalLoanAccount.CurrentBalance), Convert.ToDouble(PersonalLoanFinancialService.InterestRate), PersonalLoanFinancialService.RemainingInstalments);
            base.View.AssertInstalmentBreakdown(loanInstalment, 57.00, CreditLifePolicy.Instalment);
        }
    }
}