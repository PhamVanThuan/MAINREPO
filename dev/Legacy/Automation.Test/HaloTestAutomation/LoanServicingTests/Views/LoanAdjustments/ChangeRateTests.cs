using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.RateChange;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class ChangeRateTests : TestBase<RateChange>
    {
        private int _legalEntityKey;
        private Automation.DataModels.Account acc;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        /// <summary>
        /// This test loads up a loan and checks that the link rate select list contains the link rates from the current
        /// credit matrix as well as the link rates from the credit matrix used for the accepted new business offer.
        /// </summary>
        [Test, Description(@"This test loads up a loan and checks that the link rate select list contains the link rates from the current
            credit matrix as well as the link rates from the credit matrix used for the accepted new business offer.")]
        public void VerifyLinkRates()
        {
            StartTest(ProductEnum.NewVariableLoan, true);
            //we need the current mortage loan link rates and the latest credit matrix link rates
            int currentMLCMKey = (from ml in acc.FinancialServices select ml.CreditMatrixKey).FirstOrDefault();
            //we need the current mortage loan link rates
            var linkRates = base.Service<ICreditMatrixService>().GetCreditMatrixMargins(currentMLCMKey);
            //we need the current credit matrix rates
            linkRates.AddRange(base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins());
            var valuelist = (from l in linkRates select l.MarginKey).ToList();
            base.View.AssertLinkRates(valuelist);
        }

        private void StartTest(ProductEnum productEnum, bool isSurchargeRateAdjustment)
        {
            acc = base.Service<IAccountService>().GetRandomMortgageLoanAccountWithRateAdjustment(productEnum, AccountStatusEnum.Open, isSurchargeRateAdjustment);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(acc.AccountKey);
            //we need to navigate to loan adjustments --> change rate
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(acc.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ChangeRate();
        }

        /// <summary>
        /// This test loads up the Change Rate screen for a VariFix loan and ensures that the rates are calculated correctly for both the fixed and variable
        /// legs once a new link rate is selected from the dropdown list, ensuring that the link rate selected is not the current link rate for the loan.
        /// </summary>
        [Test, Description(@"This test loads up the Change Rate screen for a VariFix loan and ensures that the rates are calculated correctly for both the fixed
        and variable legs once a new link rate is selected from the dropdown list, ensuring that the link rate selected is not the current link rate for the loan.")]
        public void VerifyRatesOnVariFixLoan()
        {
            StartTest(ProductEnum.VariFixLoan, true);
            var fixedML = (from ml in acc.FinancialServices
                           where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                           select ml).FirstOrDefault();
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.VerifyFixedRates(fixedML.MarketRate, fixedML.RateAdjustment, (decimal)linkRate.Value);
            base.View.VerifyVariableRates(variableML.MarketRate, variableML.RateAdjustment, (decimal)linkRate.Value);
        }

        /// <summary>
        /// This test loads up the Change Rate screen for a variable loan and ensures that the rates are calculated correctly for both the variable
        /// legs once a new link rate is selected from the dropdown list, ensuring that the link rate selected is not the current link rate for the loan.
        /// </summary>
        [Test, Description(@"This test loads up the Change Rate screen for a variable loan and ensures that the rates are calculated correctly for both the variable
        legs once a new link rate is selected from the dropdown list, ensuring that the link rate selected is not the current link rate for the loan.")]
        public void VerifyRatesOnVariableLoan([Values(false, true)] bool positveRateAdjustment)
        {
            StartTest(ProductEnum.NewVariableLoan, positveRateAdjustment);
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.VerifyVariableRates(variableML.MarketRate, variableML.RateAdjustment, (decimal)linkRate.Value);
        }

        /// <summary>
        /// Once the rate change has been completed, a 920 and 940 transaction should exist against the variable loan financial service
        /// </summary>
        [Test, Description(@"Once the rate change has been completed, a 920 and 940 transaction should exist against the variable loan financial service")]
        public void ChangeRatePostsTransactions()
        {
            StartTest(ProductEnum.NewVariableLoan, true);
            var transactionDate = DateTime.Now.AddMinutes(-1);
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.ClickChangeRate();
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InterestRateChange, false);
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InstallmentChange, false);
        }

        /// <summary>
        /// Once the rate change has been completed the financialService.Payment field should have been updated to reflect the new instalment
        /// </summary>
        [Test, Description(@"Once the rate change has been completed the financialService.Payment field should have been updated to reflect the new instalment")]
        public void ChangeRateRecalculatesInstalment()
        {
            StartTest(ProductEnum.Edge, true);
            string transactionDate = DateTime.Now.ToString();
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.ClickChangeRate();
            double newInstalment = base.Service<IAccountService>().GetFinancialServicePaymentByType(acc.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            Assert.That(variableML.Payment != newInstalment, "Instalment has not changed.");
        }

        /// <summary>
        /// Once the rate change has been completed the link rate for the financial service should have been updated to reflect the link rate selected from
        /// the dropdown list.
        /// </summary>
        [Test, Description(@"Once the rate change has been completed the link rate for the financial service should have been updated to reflect the link rate
        selected from the dropdown list.")]
        public void ChangeRateChangesLinkRate()
        {
            StartTest(ProductEnum.NewVariableLoan, true);
            string transactionDate = DateTime.Now.ToString();
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.ClickChangeRate();
            //we need the new link rate
            acc = base.Service<IAccountService>().GetAccountByKey(acc.AccountKey);
            variableML = (from ml in acc.FinancialServices
                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                          select ml).FirstOrDefault();
            Assert.That(variableML.LinkRate == linkRate.Value);
        }

        /// <summary>
        /// Changing the rate on a VariFix loan should post 920 and 940 transactions on both the fixed and variable legs of the loan, as well as adjust the
        /// payments on both legs.
        /// </summary>
        [Test, Description(@"Changing the rate on a VariFix loan should post 920 and 940 transactions on both the fixed and variable legs of the loan, as well as
        adjust the payments on both legs.")]
        public void ChangeRateOnVarifixChangesBothLegs()
        {
            StartTest(ProductEnum.VariFixLoan, true);
            var transactionDate = DateTime.Now.AddMinutes(-4);
            var variableML = (from ml in acc.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            var fixedML = (from ml in acc.FinancialServices
                           where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                           select ml).FirstOrDefault();
            var linkRate = (from rates in base.Service<ICreditMatrixService>().GetLatestCreditMatrixMargins()
                            where rates.Value != variableML.LinkRate
                            select rates).FirstOrDefault();
            base.View.ChangeLinkRate(linkRate.MarginKey);
            base.View.ClickChangeRate();
            //check the fixed leg
            TransactionAssertions.AssertTransactionExists(fixedML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InterestRateChange, false);
            TransactionAssertions.AssertTransactionExists(fixedML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InstallmentChange, false);
            //check the variable leg
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InterestRateChange, false);
            TransactionAssertions.AssertTransactionExists(variableML.FinancialServiceKey, transactionDate, TransactionTypeEnum.InstallmentChange, false);
            //check payment variable
            double newVariablePayment = base.Service<IAccountService>().GetFinancialServicePaymentByType(acc.AccountKey, FinancialServiceTypeEnum.VariableLoan);
            //check payment fixed
            double newFixedPayment = base.Service<IAccountService>().GetFinancialServicePaymentByType(acc.AccountKey, FinancialServiceTypeEnum.FixedLoan);
            Assert.That(variableML.Payment != newVariablePayment && fixedML.Payment != newFixedPayment, "Rate changes were not made on both legs of the Varifix Acc");
        }
    }
}