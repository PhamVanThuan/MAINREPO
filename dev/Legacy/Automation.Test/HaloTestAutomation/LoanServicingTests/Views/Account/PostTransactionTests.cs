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
using System.Collections.Generic;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class PostTransactionTests : TestBase<PostTransaction>
    {
        private Automation.DataModels.Account VariableLoanAccount;
        private Automation.DataModels.Account VariFixAccount;

        /// <summary>
        /// We need to find an open Variable Loan and an open VariFix loan that can be used to post transactions against in our tests.
        /// </summary>
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            //the variable loan account to be used in our tests
            VariableLoanAccount = base.Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum.NewVariableLoan, AccountStatusEnum.Open);
            //the varifix loan accuont to be used in our tests.
            VariFixAccount = base.Service<IAccountService>().GetRandomMortgageLoanAccountWithPositiveBalance(ProductEnum.VariFixLoan, AccountStatusEnum.Open);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        /// <summary>
        /// This test will retrieve the list of transactions that the ArrearsManagement group has access to. Our test user, HaloUser, is part of this group so
        /// should be able to see the transactions that the ad user group is allowed to access when using the post transactions screen.
        /// </summary>
        [Test, Sequential]
        public void TransactionTypeDropdownFiltersBasedOnTransactionTypeDataAccess(
                [Values(TransactionTypeEnum.InterestDebit, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            //HaloUser is part of the ArrearsManagement group on our domain therefore the user have access to the same set of transactions
            bool isArrearTran = GoToPostTransactions(VariableLoanAccount.AccountKey, transactionType);
            var enumValues = Enum.GetValues(typeof(TransactionTypeEnum));
            var castedTransactionTypes = (from enumValue in enumValues.Cast<int>()
                                          select enumValue).ToArray<int>();
            var transactionTypeCollection =
                Service<ILoanTransactionService>().GetTransactionDataAccess("ArrearsManagement",
                                                                                castedTransactionTypes);
            transactionTypeCollection = from transactionTypeEntry in transactionTypeCollection
                                        where transactionTypeEntry.ScreenBatch == 1
                                        select transactionTypeEntry;
            var transactionTypeToInsert = (from t in transactionTypeCollection
                                           where
                                               Service<ILoanTransactionService>().IsTransactionAnArrearTransaction(t.TransactionTypeKey)
                                               == isArrearTran
                                           select t).FirstOrDefault();
            base.View.AssertTransactionTypes(
                new List<int> { (int)transactionTypeToInsert.TransactionTypeKey }, atLeastOneMatch: true);
            base.View.AssertControlsValid();
        }

        /// <summary>
        /// Ensures that a transaction type and an effective date greater than the first of this month is selected before the user is allowed to post a transaction
        /// </summary>
        [Test, Sequential]
        public void PostTransactionValidationTest(
                [Values(TransactionTypeEnum.InterestDebit, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey, transactionType);
            base.View.ClickPost();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a transaction type.");
            base.View.SelectTransaction(transactionType);
            base.View.ClickPost();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a valid Effective Date", "Please enter an amount greater than zero");
            base.View.SelectTransaction(transactionType);
            base.View.PostLoanTransaction(transactionType, 2000, DateTime.Now.Subtract(TimeSpan.FromDays(31)), "ref");

            //Build up a date string to assert
            var dateStr = DateTime.Now.ToString("dd/MM/yyyy");
            var dateStrSplit = dateStr.Split('/');
            dateStrSplit[0] = "01";
            var expectedDateStr = String.Join("/", dateStrSplit);           

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"The Effective Date must be on or after {0}", expectedDateStr));
            base.View.ClickPost();
        }

        /// <summary>
        /// The 'Debit - Non Performing Interest' transaction cannot be posted against a loan that is not currently marked as a non-performing loan.
        /// </summary>
        [Test, Sequential]
        public void CannotPostNonPerformingTransactionsOnAPerformingLoan(
            [Values(TransactionTypeEnum.DebitNonPerformingInterest, TransactionTypeEnum.NonPerformingInterest)] TransactionTypeEnum transactionType)
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(false, false, false, (int)ProductEnum.VariableLoan,
                (int)ProductEnum.NewVariableLoan);
            GoToPostTransactions(account.AccountKey);
            var variableML = (from ml in account.FinancialServices
                              where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                              select ml).FirstOrDefault();
            base.View.PostLoanTransaction(transactionType, 500, DateTime.Now, "Should not post", variableML.FinancialServiceKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "This transaction can only be posted to a loan that is currently marked as non-performing");
        }

        /// <summary>
        /// When posting a reversal of non performing interest, the total of all 236,966 and 967 transactions needs to be zero to allow the transaction to
        /// be posted.
        /// </summary>
        [Test]
        public void CanPostReverseNonPerformingInterestIfTotalIsEqualToSuspendInterestBalance()
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan,
                    (int)ProductEnum.NewVariableLoan);
            GoToPostTransactions(account.AccountKey);
            //we need the suspended interest balance
            var suspendedInterestBalance = (from ml in account.FinancialServices where ml.SuspendedInterest != null select ml.SuspendedInterest.Amount).Sum();
            var suspendedInterestFSKey = (from ml in account.FinancialServices select ml.SuspendedInterest.FinancialServiceKey).FirstOrDefault();
            var fsKey = (from ml in account.FinancialServices
                         where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                         select ml.FinancialServiceKey).FirstOrDefault();
            base.View.PostLoanTransaction(TransactionTypeEnum.ReverseNonPerformingInterest, suspendedInterestBalance, DateTime.Now, "Non Performing Reversal", fsKey);
            //check tx posted
            TransactionAssertions.AssertTransactionExists(suspendedInterestFSKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.ReverseNonPerformingInterest, false, suspendedInterestBalance);
        }

        /// <summary>
        /// When posting a payment transaction against a VariFix loan we expect there to be no financial service dropdown available for selection. Once
        /// the payment transaction has been posted we then expect the transaction exist on both legs of the VariFix loan.
        /// </summary>
        [Test]
        public void PostPaymentTransactionAgainstVarifixSplitsAcrossLegs()
        {
            GoToPostTransactions(VariFixAccount.AccountKey);
            base.View.SelectTransaction(TransactionTypeEnum.PrePayment320);
            base.View.AssertFinancialServiceDropdownExists(false);
            base.View.PostLoanTransaction(TransactionTypeEnum.PrePayment320, 25000.00M, DateTime.Now, "Payment Split Test");
            //the transaction should exist against both legs
            var fixedLeg = (from ml in VariFixAccount.FinancialServices
                            where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                            select ml).FirstOrDefault();
            var variableLeg = (from ml in VariFixAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            TransactionAssertions.AssertTransactionExists(fixedLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.PrePayment320, false);
            TransactionAssertions.AssertTransactionExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.PrePayment320, false);
        }

        /// <summary>
        /// This test will post a transaction against the fixed leg and the the variable leg of a VariFix loan, ensuring that the transaction is posted
        /// </summary>
        [Test, Sequential]
        public void PostTransactionAgainstFixedLegOfVariFixLoan(
            [Values(TransactionTypeEnum.InterestDebit, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            bool isArrearTran = GoToPostTransactions(VariFixAccount.AccountKey, transactionType);
            //we need the list to pass to the assertion
            var fsKeyList = (from ml in VariFixAccount.FinancialServices
                             where ml.FinancialServiceTypeKey == 1 || ml.FinancialServiceTypeKey == 2
                             select ml.FinancialServiceKey).ToList();
            base.View.SelectTransaction(transactionType);
            base.Browser.DomContainer.WaitForComplete();
            base.View.AssertFinancialServiceDropdownList(fsKeyList);
            //we want to post against the fixed leg
            var fixedLeg = (from ml in VariFixAccount.FinancialServices
                            where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.FixedLoan
                            select ml).FirstOrDefault();
            var variableLeg = (from ml in VariFixAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            decimal txnAmount = 500.00M;
            base.View.PostLoanTransaction(transactionType, txnAmount, DateTime.Now, "VariFix Fixed Leg Test", fixedLeg.FinancialServiceKey);
            TransactionAssertions.AssertTransactionExists(fixedLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-5), transactionType, false, (decimal)txnAmount);
            //check it doesnt exist against the variable leg
            TransactionAssertions.AssertFinancialTransactionNotExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-5), transactionType);
        }

        /// <summary>
        /// When we post a payment transaction on a variable loan we expect there to be no financial service dropdown to select from. Once the transaction is posted
        /// we expect the transaction to exist on the variable leg of the loan.
        /// </summary>
        [Test]
        public void PostPaymentTransactionAgainstVariableLoan()
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey);
            base.View.SelectTransaction(TransactionTypeEnum.PrePayment320);
            base.View.AssertFinancialServiceDropdownExists(false);
            base.View.PostLoanTransaction(TransactionTypeEnum.PrePayment320, 25000.00M, DateTime.Now, "Payment Split Test");
            var variableLeg = (from ml in VariableLoanAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            TransactionAssertions.AssertTransactionExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.PrePayment320, false,
                25000.00M);
        }

        /// <summary>
        /// Wehen there is only a single financial service linked to the account, such as in the case of a Variable Loan, then there should only be single
        /// record in the financial service dropdown and it should have been selected by default.
        /// </summary>
        [Test, Sequential]
        public void FinancialServiceDropDownDefaultsWhenOnlyOneFinancialServiceExists(
                [Values(TransactionTypeEnum.InterestDebit, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey, transactionType);
            var variableLeg = (from ml in VariableLoanAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            base.View.SelectTransaction(transactionType);
            base.View.AssertFinancialServiceDefaultSelection(variableLeg.FinancialServiceKey.ToString());
            base.View.AssertFinancialServiceDropdownList(new List<int> { variableLeg.FinancialServiceKey });
        }

        /// <summary>
        /// The financial service dropdown has to have a value selected when the user tries to post a transaction
        /// </summary>
        [Test, Sequential]
        public void FinancialServiceDropdownMandatory(
                [Values(TransactionTypeEnum.InterestDebit, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            GoToPostTransactions(VariFixAccount.AccountKey, transactionType);
            base.View.PostLoanTransaction(transactionType, 500.00M, DateTime.Now.AddMinutes(-5), "Should not post");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a financial service.");
        }

        /// <summary>
        /// A user should not be allowed to post a transaction with a zero value from the front end. A warning should be displayed to the user.
        /// </summary>
        [Test, Sequential]
        public void CannotPostZeroTransaction(
                [Values(TransactionTypeEnum.PrePayment320, TransactionTypeEnum.InstalmentPaymentArrearSettlement)] TransactionTypeEnum transactionType
            )
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey, transactionType);
            base.View.PostLoanTransaction(transactionType, 0.00M, DateTime.Now, "Zero transaction test");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter an amount greater than zero");
        }

        /// <summary>
        /// When posting a reversal of non performing interest, you cannot reverse more suspended interest than the current balance on the financial service.
        /// In this test we will try and post more than the current balance of the suspended interest financial service, which should result in a
        /// warning message being received. The test is repeated for the 'DebitNonPerformingInterest' and 'ReverseNonPerformingInterest' transaction types.
        /// </summary>
        [Test, Sequential]
        public void CannotReverseMoreSuspendedInterestThanTheCurrentSuspendedInterestBalance(
            [Values(TransactionTypeEnum.DebitNonPerformingInterest, TransactionTypeEnum.ReverseNonPerformingInterest)] TransactionTypeEnum transactionType)
        {
            var account = base.Service<IAccountService>().GetAccountForNonPerformingLoanTests(true, false, false, (int)ProductEnum.VariableLoan,
                    (int)ProductEnum.NewVariableLoan);
            GoToPostTransactions(account.AccountKey);
            //we need the suspended interest balance
            var suspendedInterestBalance = (from ml in account.FinancialServices where ml.SuspendedInterest != null select ml.SuspendedInterest.Amount).Sum() + 0.02M;
            var fsKey = (from ml in account.FinancialServices
                         where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                         select ml.FinancialServiceKey).FirstOrDefault();
            base.View.PostLoanTransaction(transactionType, suspendedInterestBalance, DateTime.Now, "Non Performing Reversal", fsKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "Unable to proceed – amount to be written off cannot exceed the current suspended interest amount");
        }

        /// <summary>
        /// When we post a payment transaction on a variable loan we expect there to be no financial service dropdown to select from. Once the transaction is posted
        /// we expect the transaction to exist on the variable leg of the loan.
        /// </summary>
        [Test, Sequential]
        public void PostNonPaymentGroupTransactionAgainstVariableLoan(
                [Values(TransactionTypeEnum.InterestCredit230, TransactionTypeEnum.ArrearsArrangementDebit)] TransactionTypeEnum transactionType,
                [Values(false, true)] bool isArrearTran)
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey, transactionType);
            base.View.SelectTransaction(transactionType);
            base.View.AssertFinancialServiceDropdownExists(true);
            var transactionAmount = 500.00M;
            var variableLeg = (from ml in VariableLoanAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            base.View.PostLoanTransaction(transactionType, transactionAmount, DateTime.Now, "Variable Loan - Non Payment Test",
                variableLeg.FinancialServiceKey);
            TransactionAssertions.AssertTransactionExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), transactionType, isArrearTran, transactionAmount);
        }

        /// <summary>
        /// The user should be allowed to post a back dated payment against an account, provided that it does not get backdated prior to the 1st of the previous month. A back dated interest credit should
        /// also be posted against the account when posting a back dated payment.
        /// </summary>
        [Test]
        public void PostBackDatedPaymentTransactionAgainstVariableLoan()
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey);
            base.View.SelectTransaction(TransactionTypeEnum.PrePayment320);
            base.View.AssertFinancialServiceDropdownExists(false);
            base.View.PostLoanTransaction(TransactionTypeEnum.PrePayment320, 25000.00M, DateTime.Now.AddMonths(-1), "Backdated Payment Test");
            var variableLeg = (from ml in VariableLoanAccount.FinancialServices
                               where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                               select ml).FirstOrDefault();
            TransactionAssertions.AssertTransactionExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.PrePayment320, false,
                25000.00M);
            TransactionAssertions.AssertTransactionExists(variableLeg.FinancialServiceKey, DateTime.Now.AddMinutes(-1), TransactionTypeEnum.BackDatedInterestCredit, false);
        }

        /// <summary>
        /// Ensures that a transaction type and an effective date greater than the first of this month is selected before the user is allowed to post a transaction
        /// </summary>
        [Test]
        public void PostTransactionBackdatedPaymentValidation()
        {
            GoToPostTransactions(VariableLoanAccount.AccountKey);
            base.View.PostLoanTransaction(TransactionTypeEnum.PrePayment320, 25000.00M, DateTime.Now.AddMonths(-2), "Backdated Payment Test");
            //Added if statement to have different date format for 01-09 dates and 10-12 dates.
            if (DateTime.Now.Month > 10 || DateTime.Now.Month == 1)
            {
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"The Effective Date must be on or after 01/{0}/{1}", 
                    DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year));
            }
            else
            {
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"The Effective Date must be on or after 01/0{0}/{1}", 
                    DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year));
            }
            }

        #region Helpers

        /// <summary>
        /// Navigates to either the Post Transaction or the Arrear Transaction screen depending on whether the transaction is an Arrear Transaction or not.
        /// </summary>
        /// <param name="accountKey">Account to use</param>
        /// <param name="transactionType">Transaction Type to post.</param>
        /// <returns></returns>
        private bool GoToPostTransactions(int accountKey, TransactionTypeEnum transactionType = TransactionTypeEnum.None)
        {
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            //check if it is an arrear transaction
            bool isArrearTran = false;
            if (transactionType != TransactionTypeEnum.None)
            {
                isArrearTran = Service<ILoanTransactionService>().IsTransactionAnArrearTransaction(transactionType);
                if (isArrearTran)
                {
                    base.Browser.Navigate<LoanServicingCBO>().PostArrearTransactions();
                }
            }
            if (!isArrearTran)
            {
                base.Browser.Navigate<LoanServicingCBO>().PostTransactions();
            }
            return isArrearTran;
        }

        #endregion Helpers
    }
}