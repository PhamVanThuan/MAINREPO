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
using WatiN.Core;

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class MarkNonPerformingTests : TestBase<MarkNonPerforming>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            // open browser with test user
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            if (new IECollection().Count == 0)
            {
                // open browser with test user
                base.Browser = new TestBrowser(TestUsers.HaloUser);
            }
            // remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        /// <summary>
        /// Verify that marking a loan as Non-Performing adds a FinancialAdjustment of Source: Suspended Interest and Type: Reversal Provision
        /// </summary>
        [Test]
        public void MarkLoanAsNonPerformingCreatesInactiveFinancialAdjustment()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.SetMarkAsNonPerformingCheckbox(true, ButtonTypeEnum.Proceed);
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans,
                FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// Verify that marking a loan as Performing marks the FinancialAdjustment of Source: Suspended Interest and Type: Reversal Provision as Cancelled
        /// </summary>
        [Test]
        public void MarkLoanAsPerformingRemovesActiveFinancialAdjustment()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.SetMarkAsNonPerformingCheckbox(false, ButtonTypeEnum.Proceed);
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans,
                FinancialAdjustmentStatusEnum.Canceled, true);
        }

        /// <summary>
        /// Verify that it is not possible to mark a Super-Lo loan as Non-Performing
        /// </summary>
        [Test]
        public void CannotMarkSuperLoLoanAsNonPerforming()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, false, false, (int)ProductEnum.SuperLo);
            SearchAndNavigate(account.AccountKey);
            base.View.AssertMarkAsNonPerformingCheckboxExists(false);
        }

        /// <summary>
        /// Verify that it is not possible to mark a VariFix loan as Non-Performing
        /// </summary>
        [Test]
        public void CannotMarkVarifixLoanAsNonPerforming()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, false, false, (int)ProductEnum.VariFixLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.AssertMarkAsNonPerformingCheckboxExists(false);
        }

        /// <summary>
        /// Verify that it is not possible to mark loan as Non-Performing when that loan has a Further Lending application in progress
        /// </summary>
        [Test]
        public void CannotMarkLoanAsNonPerformingWhenFurtherLendingInProgress()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, true, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.SetMarkAsNonPerformingCheckbox(true, ButtonTypeEnum.Proceed);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "This account has a further loan /re-advance in progress. Please refer this account to Litigation");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void PresenterDisplaysCorrectMTDInterestAmount()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            var mtdInterest = (from ml in account.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml.MTDInterest).FirstOrDefault();
            base.View.AssertMTDInterestValueVariableLeg(mtdInterest);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void PresenterDisplaysCorrectMTDInterestAmountForVariFix()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariFixLoan, (int)ProductEnum.VariFixLoan);
            SearchAndNavigate(account.AccountKey);
            var mtdInterestVariable = (from ml in account.FinancialServices
                                       where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                       select ml.MTDInterest).FirstOrDefault();
            var mtdInterestFixed = (from ml in account.FinancialServices
                                    where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                                    select ml.MTDInterest).FirstOrDefault();
            //check variable
            base.View.AssertMTDInterestValueVariableLeg(mtdInterestVariable);
            //check fixed
            base.View.AssertMTDInterestValueFixedLeg(mtdInterestFixed);
        }

        /// <summary>
        /// Verify that it is not possible to mark a loan as Performing if any of the following DetailTypes exist against the loan: (11,180,275,299,592,227,581,582,583,584,590)
        /// </summary>
        [Test]
        public void MarkNonPerformingDetailTypeCheck()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, true, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.SetMarkAsNonPerformingCheckbox(false, ButtonTypeEnum.Proceed);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("This account has detail types that prevent it from being set as performing again. Please refer this account to Litigation");
        }

        /// <summary>
        /// Verify that it is not possible to reprocess a loan as Non-Performing if that loan is currently marked as Non-Performing
        /// </summary>
        [Test]
        public void CannotMarkNonPerformingLoanAsNonPerforming()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.AssertMarkAsNonPerformingCheckboxExists(true);
            base.View.AssertMarkAsNonPerformingCheckboxChecked(true);
            base.View.AssertProceedButtonExists(false);
        }

        /// <summary>
        /// When reinstating a loan as performing we should post 2 transactions, one for the MTD interest and then another reversal for the
        /// total of all the suspended interest for the loan
        /// </summary>
        [Test]
        public void MarkLoanAsPerformingPostsCorrectTransactions()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan,
                (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            var mortgageLoan = (from ml in account.FinancialServices
                                where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                select ml).FirstOrDefault();
            var suspendedInterest = mortgageLoan.SuspendedInterest;
            var date = DateTime.Now.AddMinutes(-1);
            base.View.SetMarkAsNonPerformingCheckbox(false, ButtonTypeEnum.Proceed);
            //check transaction 966 for MTD interest posted
            TransactionAssertions.AssertTransactionExists(suspendedInterest.FinancialServiceKey, date, TransactionTypeEnum.NonPerformingInterest, false,
                mortgageLoan.MTDInterest);
            var reversalValue = suspendedInterest.Amount + mortgageLoan.MTDInterest;
            //reversal
            TransactionAssertions.AssertTransactionExists(suspendedInterest.FinancialServiceKey, date, TransactionTypeEnum.ReverseNonPerformingInterest, false, reversalValue);
            //account closed
            TransactionAssertions.AssertTransactionExists(suspendedInterest.FinancialServiceKey, date, TransactionTypeEnum.CloseAccount, false);
            //reversal adjustment removed
            TransactionAssertions.AssertTransactionExists(mortgageLoan.FinancialServiceKey, date, TransactionTypeEnum.CancelReversalAdjustment, false);
        }

        /// <summary>
        /// This test will ensure the presenter displays the correct suspended interest to date amount.
        /// </summary>
        [Test]
        public void PresenterDisplaysCorrectSuspendedInterestToDateAmount()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            var suspendedInterest = (from ml in account.FinancialServices
                                     where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan && ml.AccountStatusKey == (int)AccountStatusEnum.Open
                                     select ml.SuspendedInterest).FirstOrDefault();
            base.View.AssertSuspendedInterestValueVariableLeg(suspendedInterest.Amount);
        }

        /// <summary>
        /// This test will ensure the presenter displays the correct suspended interest to date for VariFix loans
        /// </summary>
        [Test]
        public void PresenterDisplaysCorrectSuspendedInterestToDateAmountForVariFix()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariFixLoan, (int)ProductEnum.VariFixLoan);
            SearchAndNavigate(account.AccountKey);
            var suspendedInterestVariable = (from ml in account.FinancialServices
                                             where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan && ml.AccountStatusKey == (int)AccountStatusEnum.Open
                                             select ml.SuspendedInterest).FirstOrDefault();
            var suspendedInterestFixed = (from ml in account.FinancialServices
                                          where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan && ml.AccountStatusKey == (int)AccountStatusEnum.Open
                                          select ml.SuspendedInterest).FirstOrDefault();
            base.View.AssertSuspendedInterestValueFixedLeg(suspendedInterestFixed.Amount);
        }

        /// <summary>
        /// This test will take a performing loan and then mark it as non-performing. This will load an inactive financial adjustment. We then
        /// need to allow the user to remove this financial adjustment in case it was erroneously loaded against an account.
        /// </summary>
        [Test]
        public void MarkPendingNonPerformingLoanAsPerforming()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, false, false, (int)ProductEnum.VariableLoan, (int)ProductEnum.NewVariableLoan);
            SearchAndNavigate(account.AccountKey);
            base.View.SetMarkAsNonPerformingCheckbox(true, ButtonTypeEnum.Proceed);
            int fadjKey = FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans,
                FinancialAdjustmentStatusEnum.Inactive, true);
            base.Browser.Navigate<LoanServicingCBO>().MarkNonPerforming();
            //try again
            base.View.SetMarkAsNonPerformingCheckbox(false, ButtonTypeEnum.Proceed);
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(account.AccountKey, FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans,
                FinancialAdjustmentStatusEnum.Canceled, true, fadjKey);
        }

        #region Helper

        /// <summary>
        /// Search for the account load it onto the CBO and navigate to the Mark Non-Performing Loan screen.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        private void SearchAndNavigate(int accountKey)
        {
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            base.Browser.Navigate<LoanServicingCBO>().MarkNonPerforming();
        }

        #endregion Helper
    }
}