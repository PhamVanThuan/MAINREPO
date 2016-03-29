using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class ChangeInstalmentTests : TestBase<ChangeInstalmentView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account account;
        private Automation.DataModels.LoanFinancialService fixedML;
        private Automation.DataModels.LoanFinancialService variableML;

        private double newVariableInstalment;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        #endregion SetupTearDown

        /// <summary>
        /// This test ensures that the user can perform an instalment recalculation and change the instalment for a variable loan.
        /// </summary>
        [Test]
        public void ChangeInstalmentVariableLoan()
        {
            SearchAndNavigate(ProductEnum.NewVariableLoan);
            var tranDate = DateTime.Now.AddMinutes(-1);
            variableML = (from ml in account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            base.View.ChangeInstalment();
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, tranDate, TransactionTypeEnum.InstallmentChange, false);
            newVariableInstalment = base.Service<IAccountService>().GetFinancialServicePaymentByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            Assert.AreNotEqual(variableML.Payment, newVariableInstalment, "Variable leg Payment was not updated with the New Instalment");
        }

        /// <summary>
        /// This test ensures that the user can perform an instalment recalculation and change the instalment for a varifix loan.
        /// </summary>
        [Test]
        public void ChangeInstalmentVarifixLoan()
        {
            SearchAndNavigate(ProductEnum.VariFixLoan);
            var tranDate = DateTime.Now.AddMinutes(-1);
            variableML = (from ml in account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            fixedML = (from ml in account.FinancialServices
                       where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                       select ml).FirstOrDefault();
            base.View.ChangeInstalment();
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, tranDate, TransactionTypeEnum.InstallmentChange, false);
            TransactionAssertions.AssertTransactionExists(fixedML.FinancialServiceKey, tranDate, TransactionTypeEnum.InstallmentChange, false);
            newVariableInstalment = base.Service<IAccountService>().GetFinancialServicePaymentByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            var newFixedInstalment = base.Service<IAccountService>().GetFinancialServicePaymentByType(account.AccountKey, FinancialServiceTypeEnum.FixedLoan);
            Assert.AreNotEqual(variableML.Payment, newVariableInstalment, "Variable leg Payment was not updated with the New Instalment");
            Assert.AreNotEqual(fixedML.Payment, newFixedInstalment, "Fixed leg Payment was not updated with the New Instalment");
        }

        /// <summary>
        /// This test ensures that the user can perform an instalment recalculation and change the interest only instalment for a variable loan.
        /// </summary>
        [Test]
        public void ChangeInstalmentInterestOnly()
        {
            account = base.Service<IAccountService>().GetRandomInterestOnlyAccount(ProductEnum.NewVariableLoan);
            Service<ILoanTransactionService>().pProcessAccountPaymentTran(account.AccountKey, TransactionTypeEnum.PrePayment320, 25000, "Post Pre Payment",
                @"sahl\tester", DateTime.Now);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeInstalment();
            var tranDate = DateTime.Now.AddMinutes(-1);
            variableML = (from ml in account.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            //determine current balance minus the sum of the child financial service balances
            double currentBalance = Service<IAccountService>().GetCurrentBalance(account.AccountKey);
            double childBalance = Service<IAccountService>().GetSumOfChildFinancialServiceBalance(account.AccountKey);
            double instalmentBalance = currentBalance - childBalance;
            decimal interestOnlyInstalment = base.Service<ICalculationsService>().CalculateInterestOnlyInstalment((decimal)instalmentBalance, variableML.InterestRate);
            base.View.ChangeInstalment();
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, tranDate, TransactionTypeEnum.InstallmentChange, false);
            double newInterestOnlyInstalment = base.Service<IAccountService>().GetFinancialServicePaymentByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            Assert.AreEqual(Math.Round(interestOnlyInstalment, 2), Math.Round(newInterestOnlyInstalment, 2), "Interest Only instalment amount is incorrect");
        }

        #region Helper

        private void SearchAndNavigate(ProductEnum productEnum)
        {
            account = base.Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(productEnum, AccountStatusEnum.Open);
            Service<ILoanTransactionService>().pProcessAccountPaymentTran(account.AccountKey, TransactionTypeEnum.PrePayment320, 25000, "Post Pre Payment",
                @"sahl\tester", DateTime.Now);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeInstalment();
        }

        #endregion Helper
    }
}