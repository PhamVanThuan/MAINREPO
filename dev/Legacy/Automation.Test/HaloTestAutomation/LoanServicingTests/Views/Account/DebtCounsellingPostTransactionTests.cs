using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class DebtCounsellingPostTransactionTests : TestBase<PostTransaction>
    {
        private Automation.DataModels.Account mortgageLoanAccount;
        private Automation.DataModels.DebtCounselling debtCounsellingCase;
        public decimal amountToPost = 5000.00M;

        protected override void OnTestFixtureSetup()
        {
            mortgageLoanAccount = base.Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum.NewVariableLoan, AccountStatusEnum.Open);
            debtCounsellingCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases().SelectRandom();
            base.OnTestFixtureSetup();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        /// <summary>
        /// A debt counselling supervisor should NOT be able to post a DebtReviewArrangementDebit transaction against an account that is NOT under
        /// debt counselling
        /// </summary>
        [Test, Description(@"A debt counselling supervisor should NOT be able to post a DebtReviewArrangementCredit transaction against an account that is NOT under
             debt counselling")]
        public void _01_PostDebtReviewArrangementCreditAccountNotUnderDebtCounselling()
        {
            GoToPostArrearTransactions(mortgageLoanAccount.AccountKey, TestUsers.DebtCounsellingSupervisor);
            base.View.PostLoanTransaction(TransactionTypeEnum.DebtReviewArrangementCredit, amountToPost, DateTime.Now, "Debt Counselling Test");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Account is not in Debt Counselling.");
        }

        /// <summary>
        /// A debt counselling supervisor should NOT be able to post a DebtReviewArrangementDebit transaction against an account that is NOT under
        /// debt counselling
        /// </summary>
        [Test, Description(@"A debt counselling supervisor should NOT be able to post a DebtReviewArrangementDebit transaction against an account that is NOT under
             debt counselling")]
        public void _02_PostDebtReviewArrangementDebitAccountNotUnderDebtCounselling()
        {
            GoToPostArrearTransactions(mortgageLoanAccount.AccountKey, TestUsers.DebtCounsellingSupervisor);
            base.View.PostLoanTransaction(TransactionTypeEnum.DebtReviewArrangementDebit, amountToPost, DateTime.Now, "Debt Counselling Test");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Account is not in Debt Counselling.");
        }

        /// <summary>
        /// A debt counselling supervisor should be able to post a DebtReviewArrangementDebit transaction against an account that is under
        /// debt counselling
        /// </summary>
        [Test, Description(@"A debt counselling supervisor should be able to post a DebtReviewArrangementCredit transaction against an account that is under
             debt counselling")]
        public void _03_PostDebtReviewArrangementCreditAccountIsUnderDebtCounselling()
        {
            GoToPostArrearTransactions(debtCounsellingCase.Account.AccountKey, TestUsers.DebtCounsellingSupervisor);
            mortgageLoanAccount = base.Service<IAccountService>().GetAccountByKey(debtCounsellingCase.Account.AccountKey);
            base.View.PostLoanTransaction(TransactionTypeEnum.DebtReviewArrangementCredit, amountToPost, DateTime.Now, "Debt Counselling Test");
            TransactionAssertions.AssertArrearTransactionExists(mortgageLoanAccount.AccountKey, TransactionTypeEnum.DebtReviewArrangementCredit);
        }

        /// <summary>
        /// A debt counselling supervisor should be able to post a DebtReviewArrangementDebit transaction against an account that is under
        /// debt counselling
        /// </summary>
        [Test, Description(@"A debt counselling supervisor should be able to post a DebtReviewArrangementDebit transaction against an account that is under
             debt counselling")]
        public void _04_PostDebtReviewArrangementDebitAccountIsUnderDebtCounselling()
        {
            GoToPostArrearTransactions(debtCounsellingCase.Account.AccountKey, TestUsers.DebtCounsellingSupervisor);
            mortgageLoanAccount = base.Service<IAccountService>().GetAccountByKey(debtCounsellingCase.Account.AccountKey);
            base.View.PostLoanTransaction(TransactionTypeEnum.DebtReviewArrangementDebit, amountToPost, DateTime.Now, "Debt Counselling Test");
            TransactionAssertions.AssertArrearTransactionExists(mortgageLoanAccount.AccountKey, TransactionTypeEnum.DebtReviewArrangementDebit);
        }

        /// <summary>
        /// This test ensures that the debt counselling supervisor has access to the DebtReviewArrangementCredit and DebtReviewArrangementDebit transactions
        /// </summary>
        [Test, Description(@"This test ensures that the debt counselling manager has access to the DebtReviewArrangementCredit and DebtReviewArrangementDebit
        transactions")]
        public void _05_CheckDebtCounsellingSupervisorTransactionTypeAccess()
        {
            GoToPostArrearTransactions(debtCounsellingCase.Account.AccountKey, TestUsers.DebtCounsellingSupervisor);
            var expectedTransactions = new List<int> { (int)TransactionTypeEnum.DebtReviewArrangementCredit, (int)TransactionTypeEnum.DebtReviewArrangementDebit };
            base.View.AssertTransactionTypes(expectedTransactions);
        }

        /// <summary>
        /// This test ensures that the debt counselling manager has access to the DebtReviewArrangementCredit and DebtReviewArrangementDebit transactions
        /// </summary>
        [Test, Description(@"This test ensures that the debt counselling manager has access to the DebtReviewArrangementCredit and DebtReviewArrangementDebit
        transactions")]
        public void _06_CheckDebtCounsellingManagerTransactionTypeAccess()
        {
            GoToPostArrearTransactions(debtCounsellingCase.Account.AccountKey, TestUsers.DebtCounsellingManager);
            var expectedTransactions = new List<int>() {
                    (int)TransactionTypeEnum.DebtReviewArrangementCredit,
                    (int)TransactionTypeEnum.DebtReviewArrangementDebit,
                    (int)TransactionTypeEnum.ArrearsArrangementCredit,
                    (int)TransactionTypeEnum.ArrearsArrangementDebit
            };
            base.View.AssertTransactionTypes(expectedTransactions);
        }

        /// <summary>
        /// This test ensures that the debt counselling consultant does not have access to Post Arrear Transactions
        /// </summary>
        [Test, Description("This test ensures that the debt counselling consultant does not have access to Post Arrear Transactions")]
        public void _07_DebtCounsellingConsultantCannotPostArrearTransactions()
        {
            base.Browser = Helper.LoginAndSearch(TestUsers.DebtCounsellingConsultant, TestUsers.Password, mortgageLoanAccount.AccountKey);
            base.Browser.Page<BasePageAssertions>().AssertFrameDoesNotContainLink("Post Arrear Transactions", "title");
        }

        private void GoToPostArrearTransactions(int accountKey, string testUser)
        {
            if (base.Browser != null)
                base.Browser.Dispose();
            base.Browser = Helper.LoginAndSearch(testUser, TestUsers.Password, accountKey);
            base.Browser.Navigate<LoanServicingCBO>().PostArrearTransactions();
        }
    }
}