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

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class ConvertStaffLoanTests : TestBase<ConvertStaffLoanView>
    {
        #region PrivateVariables

        private Automation.DataModels.Account account;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        #endregion SetupTearDown

        #region Validation

        /// <summary>
        /// This test ensures that if the expected staff home loan detail type has not been added to the loan account a validation message is
        /// displayed when trying to convert the loan account to a staff loan.
        /// </summary>
        [Test]
        public void ValidateExpectedDetailType()
        {
            RandomAccountToConvert(ProductEnum.NewVariableLoan, 117);
            SearchAndNavigate(account.AccountKey);
            base.View.Convert();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("The AccountKey does not have a Staff Home Loan detailtype");
        }

        /// <summary>
        /// This test ensures that a loan account can't be uncoverted form a staff loan if it is not currently an active staff loan.
        /// </summary>
        [Test]
        public void ValidateNotActiveStaffLoan()
        {
            RandomAccountToConvert(ProductEnum.NewVariableLoan, 117);
            SearchAndNavigate(account.AccountKey);
            base.View.AssertUnconvertDisabled();
        }

        #endregion Validation

        #region ConvertStaffLoan

        /// <summary>
        /// This test ensures that a loan account can be converted to a staff loan. It checks that the expected financial transaction is posted and
        /// that the staff loan financial adjustment is added.
        /// </summary>
        [Test]
        public void ConvertStaffLoan()
        {
            RandomAccountToConvert(ProductEnum.NewVariableLoan, 117);
            SearchAndNavigate(account.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.StaffHomeLoan, account.AccountKey);
            base.View.Convert();
            //Check Opt In transaction posted against account
            TransactionAssertions.AssertLoanTransactionExists(account.AccountKey, TransactionTypeEnum.StaffOptIn);

            //check that Financial Adjustment that is added is inactive
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.Staff_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Inactive, true);

            //button disabled
            base.View.AssertConvertDisabled();
        }

        #endregion ConvertStaffLoan

        #region UnConvertStaffLoan

        /// <summary>
        /// This test ensures that an acitve staff loan account can be unconverted. It checks that the expected financial transaction is posted,
        /// the detail type is removed and staff loan financial adjusetment is set to inactive.
        /// </summary>
        [Test]
        public void UnConvertStaffLoan()
        {
            RandomAccountToUnconvert(ProductEnum.NewVariableLoan, 117);
            SearchAndNavigate(account.AccountKey);
            var tranDate = DateTime.Now;
            base.View.UnConvert();
            //BuildingBlocks.TransactionAssertions.AssertLoanTransactionExists(accountKey, tranDate, LoanTransactionType.InterestRateChange);
            // BuildingBlocks.TransactionAssertions.AssertLoanTransactionExists(accountKey, tranDate, LoanTransactionType.StaffOptOut);
            AccountAssertions.AssertDetailType(account.AccountKey, DetailTypeEnum.StaffHomeLoan, DetailClassEnum.LoanIdentification, false);
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.Staff_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled, true);
        }

        #endregion UnConvertStaffLoan

        #region Helper

        /// <summary>
        /// Search for the account load it onto the CBO and navigate to the Convert Staff Loan screen.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        private void SearchAndNavigate(int accountKey)
        {
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().ConvertStaffLoan();
        }

        /// <summary>
        /// Fetches a random loan account for the specified product and spv.
        /// </summary>
        /// <param name="productKey">ProductKey</param>
        /// <param name="spvKey">SPVKey</param>
        private void RandomAccountToConvert(ProductEnum productKey, int spvKey)
        {
            account = base.Service<IAccountService>().GetRandomAccountInSPVWithoutDetailType(productKey, spvKey, DetailTypeEnum.StaffHomeLoan, DetailClassEnum.LoanIdentification);
        }

        /// <summary>
        /// Fetches a random active staff loan account for the specified product.
        /// </summary>
        /// <param name="productKey">ProductKey</param>
        private void RandomAccountToUnconvert(ProductEnum productKey, int spvKey)
        {
            account = base.Service<IAccountService>().GetRandomAccountInSPVWithDetailType(productKey, spvKey, DetailTypeEnum.StaffHomeLoan, DetailClassEnum.LoanIdentification);
        }

        #endregion Helper
    }
}