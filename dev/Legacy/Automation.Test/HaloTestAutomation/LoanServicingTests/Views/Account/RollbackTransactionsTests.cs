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
    public class RollbackTransactionsTests : TestBase<RollbackTransaction>
    {
        /// <summary>
        /// We need to find an open Variable Loan and an open VariFix loan that can be used to post transactions against in our tests.
        /// </summary>
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

        /// <summary>
        ///This test will find a new variable loan with a transaction posted against it that can be rolled back. It will then roll back the transaction
        ///and ensure that the correction transaction is posted.
        /// </summary>
        [Test]
        public void RollbackFinancialTransaction()
        {
            var results = Service<ILoanTransactionService>().GetFinancialTransactionForRollback(ProductEnum.NewVariableLoan);
            int accountKey = results.Rows(0).Column(0).GetValueAs<int>();
            int financialTransactionKey = results.Rows(0).Column(1).GetValueAs<int>();
            var transactionType = Service<ILoanTransactionService>().GetTransactionTypeDetails(results.Rows(0).Column(2).GetValueAs<int>());
            int financialServiceKey = results.Rows(0).Column(3).GetValueAs<int>();
            RollbackTransactionByAccountKey(accountKey, financialTransactionKey, false);
            // check that transaction has been rolled back
            TransactionAssertions.AssertTransactionExists(financialServiceKey, DateTime.Now.AddMinutes(-1), transactionType.ReversalTransactionTypeKey,
                false);
        }

        /// <summary>
        /// When a payment transaction is selected for rollback on a VariFix loan, both the transactions posted against the variable financial service and the
        /// fixed financial service should be rolled back. This test finds a VariFix account, posts a pre payment transaction in the background and then
        /// tries to rollback the transaction.
        /// </summary>
        [Test]
        public void RollbackFinancialTransactionOnVarifix_RollsBackRelatedTransactions()
        {
            //we need a varifix account for the test
            var account = Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum.VariFixLoan, AccountStatusEnum.Open);
            //post a pre-payment, as this transaction will get split across the legs
            var results = Service<ILoanTransactionService>().pProcessAccountPaymentTran(account.AccountKey, TransactionTypeEnum.PrePayment320, 20000, "Test", @"SAHL\HaloUser", DateTime.Now);
            int financialTransactionKey = (from r in results select r.Column("FinancialTransactionKey").GetValueAs<int>()).FirstOrDefault();
            RollbackTransactionByAccountKey(account.AccountKey, financialTransactionKey, false);
            //check against both financial services
            int variableFSKey = account.FinancialServices.Where(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan)
                                .Select(b => b.FinancialServiceKey).FirstOrDefault();
            int fixedFSKey = account.FinancialServices.Where(x => x.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan)
                .Select(b => b.FinancialServiceKey).FirstOrDefault();

            TransactionAssertions.AssertTransactionExists(variableFSKey, DateTime.Now.AddMinutes(-4), TransactionTypeEnum.PrePaymentCorrection, false);
            TransactionAssertions.AssertTransactionExists(fixedFSKey, DateTime.Now.AddMinutes(-4), TransactionTypeEnum.PrePaymentCorrection, false);
        }

        /// <summary>
        /// This test will find an arrears transaction that is allowed to be rolled back and then roll back the transaction ensuring that the
        /// appropriate reversal arrears transaction is posted against the arrears financial service.
        /// </summary>
        [Test]
        public void RollbackArrearTransaction()
        {
            var results = Service<ILoanTransactionService>().GetArrearTransactionForRollback(ProductEnum.NewVariableLoan);
            int accountKey = results.Rows(0).Column(0).GetValueAs<int>();
            int arrearTransactionKey = results.Rows(0).Column(1).GetValueAs<int>();
            var transactionType = Service<ILoanTransactionService>().GetTransactionTypeDetails(results.Rows(0).Column(2).GetValueAs<int>());
            int financialServiceKey = results.Rows(0).Column(3).GetValueAs<int>();
            RollbackTransactionByAccountKey(accountKey, arrearTransactionKey, true);
            var Account = Service<IAccountService>().GetAccountByKey(accountKey);
            int fsKey = (from ml in Account.FinancialServices
                         where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                         select ml.FinancialServiceKey).FirstOrDefault();
            // check that transaction has been rolled back
            TransactionAssertions.AssertTransactionExists(fsKey, DateTime.Now.AddMinutes(-1), transactionType.ReversalTransactionTypeKey,
                true);
        }

        /// <summary>
        /// Certain transactions are not allowed to be rolled back. One of these transactions is the Interest Change. This test will try to roll back an
        /// Interest Rate Change transaction, ensuring the a warning message is displayed and the user is not navigated to the next screen.
        /// </summary>
        [Test]
        public void RollingBackATransactionThatCannotBeRolledBackResultsInWarning()
        {
            var results = Service<ILoanTransactionService>().GetAccountWithLoanTransactionAgainstParentFinancialService(TransactionTypeEnum.InterestRateChange);
            int accountKey = results.Rows(0).Column(0).GetValueAs<int>();
            int transactionKey = results.Rows(0).Column(1).GetValueAs<int>();
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().RollbackTransactions();
            base.View.SelectFinancialTransactionAndRollback(transactionKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This transaction can not be rolled back.");
            //still on the grid screen
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("TransactionRollback");
        }

        #region Helper

        /// <summary>
        /// Navigates to the rollback transactions screen and rolls back the financialTransaction provided.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="transactionKey"></param>
        private void RollbackTransactionByAccountKey(int accountKey, int transactionKey, bool goToArrearsRollback)
        {
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            if (goToArrearsRollback)
            {
                base.Browser.Navigate<LoanServicingCBO>().RollbackArrearTransaction();
            }
            else
            {
                base.Browser.Navigate<LoanServicingCBO>().RollbackTransactions();
            }
            base.View.SelectFinancialTransactionAndRollback(transactionKey);
            base.Browser.Page<TransactionRollbackConfirm>().ConfirmRollBack();
        }

        #endregion Helper
    }
}