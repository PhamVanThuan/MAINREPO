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
    public class ChangeTermTests : PersonalLoansWorkflowTestBase<PersonalLoanChangeTerm>
    {
        private Automation.DataModels.Account CreditLifePolicy { get; set; }

        private Automation.DataModels.LoanFinancialService PersonalLoanFinancialService { get; set; }

        private int MaxTotalTerm = 60;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoansClientServiceManager);
        }

        protected override void OnTestStart()
        {
            base.GenericKey = Helper.FindPersonalLoanAccount(true);
            base.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(this.GenericKey);
            this.PersonalLoanFinancialService = PersonalLoanAccount.FinancialServices.FirstOrDefault(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.PersonalLoan);
            var results = Service<IAccountService>().GetOpenRelatedAccountsByProductKey(PersonalLoanAccount.AccountKey, Common.Enums.ProductEnum.SAHLCreditProtectionPlan);
            CreditLifePolicy = Service<IAccountService>().GetAccountByKey(results.Rows(0).Column("AccountKey").GetValueAs<int>());
            base.SearchAndLoadAccount();
            base.Browser.Navigate<LoanServicingCBO>().ChangeTerm();
        }

        [Test, Sequential, Description("Change the term on a personal loan account, ensuring that the term is updated to new value. In this test a term increase is performed.")]
        public void ChangePersonalLoanTerm([Values(1, -1)] int termToAdd)
        {
            int newRemainingTerm = PersonalLoanFinancialService.RemainingInstalments + termToAdd;
            int newTerm = PersonalLoanFinancialService.Term + termToAdd;
            //change the term
            string comment = "Change Term Automation Test";
            base.View.ChangeTerm(newRemainingTerm, comment);
            base.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(PersonalLoanAccount.AccountKey);
            PersonalLoanFinancialService = PersonalLoanAccount.FinancialServices.FirstOrDefault(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.PersonalLoan);
            //check remaining term
            Assert.That(PersonalLoanFinancialService.RemainingInstalments == newRemainingTerm,
                string.Format(@"The new remaining term for Acc: {0} was not updated to {1}", PersonalLoanAccount.AccountKey, newRemainingTerm));
            //check term
            Assert.That(PersonalLoanFinancialService.Term == newTerm,
                string.Format(@"The new term for Acc: {0} was not updated to {1}", PersonalLoanAccount.AccountKey, newTerm));
            //should have loan transactions for instalment change and the term change
            TransactionAssertions.AssertLoanTransactionExists(base.PersonalLoanAccount.AccountKey, FinancialServiceTypeEnum.PersonalLoan, Common.Enums.TransactionTypeEnum.InstallmentChange);
            TransactionAssertions.AssertLoanTransactionExists(base.PersonalLoanAccount.AccountKey, FinancialServiceTypeEnum.PersonalLoan, Common.Enums.TransactionTypeEnum.TermChange);
            //should have a memo record
            MemoAssertions.AssertLatestMemoInformation(PersonalLoanAccount.AccountKey, GenericKeyTypeEnum.Account_AccountKey, MemoTable.Memo, string.Format(@"Term Change: {0}", comment));
            //should have a stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(PersonalLoanAccount.AccountKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ChangeTerm);
        }

        [Test, Description("If a comment is not provided then the term change cannot be processed. Also a term change cannot be processed without entering a new remaining term value greater than zero.")]
        public void CannotProcessTermChangeRulesValidation()
        {
            base.View.PopulateFields(0, "Comment");
            base.View.ClickCalculate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The remaining term must be between 1 and 60 months.");
            base.View.PopulateFields(PersonalLoanFinancialService.RemainingInstalments + 1, string.Empty);
            base.View.ClickCalculate();
            base.View.ClickProcessTermChange();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Term Change Request, please add Comment.");
        }

        [Test, Description("Checks that the instalment breakdown is correctly populated when clicking the calculate button")]
        public void CheckTermChangeInstalmentBreakdown()
        {
            var newTerm = PersonalLoanFinancialService.RemainingInstalments + 1;
            base.View.PopulateFields(newTerm, string.Empty);
            base.View.ClickCalculate();
            var loanInstalment = Service<ICalculationsService>().CalculateLoanInstalment(Convert.ToDouble(PersonalLoanAccount.CurrentBalance), Convert.ToDouble(PersonalLoanFinancialService.InterestRate), newTerm);
            base.View.AssertInstalmentBreakdown(loanInstalment, 57.00, CreditLifePolicy.Instalment);
        }

        [Test, Description("Test ensures that the correct value is displayed on the screen and that a rule is in place to prevent the user from exceeding the max value ")]
        public void CheckMaxRemainingTermAllowedRule()
        {
            var maxAllowedRemainingTerm = MaxTotalTerm - (PersonalLoanFinancialService.Term - PersonalLoanFinancialService.RemainingInstalments);
            base.View.AssertMaxTermAllowed(maxAllowedRemainingTerm);
            //populate with a value higher than max
            base.View.PopulateFields(maxAllowedRemainingTerm + 1, "Should not change term");
            base.View.ClickCalculate();
            string message = maxAllowedRemainingTerm >= 60 ? "The remaining term must be between 1 and 60 months." : string.Format(@"The new remaining term cannot exceed {0} months.", maxAllowedRemainingTerm);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(message);
            //also cannot be the same value as the current remaining term
            base.View.PopulateFields(PersonalLoanFinancialService.RemainingInstalments, "Should not change term");
            base.View.ClickCalculate();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The new remaining term cannot be the same as the current term");
        }
    }
}