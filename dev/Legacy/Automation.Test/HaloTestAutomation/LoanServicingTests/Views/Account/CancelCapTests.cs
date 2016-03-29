using Automation.DataAccess;
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

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class CancelCapTests : TestBase<CancelCapView>
    {
        private Automation.DataModels.Account _account = null;
        private QueryResultsRow _results = null;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //navigate
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            _results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(
                FinancialAdjustmentTypeSourceEnum.CAP2_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Active);
            _account = base.Service<IAccountService>().GetAccountByKey(_results.Column("accountKey").GetValueAs<int>());
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(_account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(_account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().CancelCap();
        }

        /// <summary>
        /// This test ensures that a user can cancel an active CAP 2 financial adjustment using the Cancel Cap screen in loan servicing
        /// The test ensures that the financial adjustment is set to cancelled and that the correct set of transactions are written against
        /// the ML financial service
        /// </summary>
        [Test]
        public void CancelCap2PostsTransactions()
        {
            var fadjKey = _results.Column("financialAdjustmentKey").GetValueAs<int>();
            var transactionDate = DateTime.Now.AddSeconds(-60);
            //cancel the cap
            base.View.CancelCap();
            //check that the adjustment is set to cancelled
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(_account.AccountKey, FinancialAdjustmentTypeSourceEnum.CAP2_FixedRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled, true, fadjKey);
            //check transactions posted
            var mortgageLoan = (from ml in _account.FinancialServices
                                where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                select ml).FirstOrDefault();
            //2004 tx
            TransactionAssertions.AssertTransactionExists(mortgageLoan.FinancialServiceKey, transactionDate, TransactionTypeEnum.CAPOptOut, false);
            //fixed rate adjustment cancellation
            TransactionAssertions.AssertTransactionExists(mortgageLoan.FinancialServiceKey, transactionDate, TransactionTypeEnum.CancelFixedRateAdjustment, false);
            //321 MTD cap prepayment
            TransactionAssertions.AssertTransactionExists(mortgageLoan.FinancialServiceKey, transactionDate, TransactionTypeEnum.PrePayment321, false);
        }

        /// <summary>
        /// A cancellation reason is required in order for a CAP 2 financial adjustment to be cancelled
        /// </summary>
        [Test]
        public void CancelCapRequiresReason()
        {
            base.View.CancelCapWithoutReason();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Cancellation Reason");
        }

        /// <summary>
        /// This test ensures that the user cannot cancel the CAP twice and that the product opt out SP returns an error code
        /// to HALO.
        /// </summary>
        [Test]
        public void CannotCancelWhenCAP2AdjustmentIsCancelled()
        {
            base.View.CancelCap();
            base.Browser.Navigate<LoanServicingCBO>().CancelCap();
            base.View.CancelCap();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("product.pCAPOptOut failed");
        }
    }
}