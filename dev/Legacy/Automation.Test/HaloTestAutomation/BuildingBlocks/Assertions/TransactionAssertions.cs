using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class TransactionAssertions
    {
        private static readonly ILoanTransactionService loanTransactionService;

        static TransactionAssertions()
        {
            loanTransactionService = ServiceLocator.Instance.GetService<ILoanTransactionService>();
        }

        /// <summary>
        /// This assertion will assert that an arrear transaction exist posted within the last minute.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="transactionType"></param>
        public static void AssertArrearTransactionExists(int accountKey, TransactionTypeEnum transactionType, int minutes = -1)
        {
            var date = DateTime.Now.AddMinutes(minutes).ToString(Formats.DateTimeFormatSQL);
            QueryResults r = loanTransactionService.GetArrearTransactionByTypeDateAndAccountKey(accountKey, date, (int)transactionType);
            Logger.LogAction(String.Format(@"Asserting that an arrear transaction of type {0} exists against account {1} with a insert date greater than {2}",
                transactionType, accountKey, date));
            Assert.True(r.HasResults, "Arrear Transaction of type {0} does not exist against account {1} with a insert date greater than {2}", transactionType, accountKey, date);
        }

        /// <summary>
        /// This assertion will check that a loan transaction record exists against on account posted within the last minute.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="tranType">LoanTransactionType</param>
        public static void AssertLoanTransactionExists(int accountKey, TransactionTypeEnum tranType, int minutes = -1)
        {
            var date = DateTime.Now.AddMinutes(minutes);
            var results = loanTransactionService.GetLoanTransactionByTypeDateAndAccountKey(accountKey, date, (int)tranType);
            Logger.LogAction(String.Format(@"Asserting that a financial transaction of type {0} exists against account {1} with an insert date greater than {2}",
                tranType, accountKey, date));
            Assert.True(results.HasResults, "Financial transaction of type: {0} does not exist against account: {1}.", tranType, accountKey);
        }

        /// <summary>
        /// This assertion should be used when you need to check that a transaction has been posted against an individual financial service.
        /// </summary>
        /// <param name="financialServiceKey">financialservicekey</param>
        /// <param name="date">transaction date</param>
        /// <param name="transactionType">transaction type</param>
        /// <param name="transactionAmount">transactio nAmount</param>
        public static void AssertTransactionExists(int financialServiceKey, DateTime date, TransactionTypeEnum transactionType, bool isArrearTransaction, decimal transactionAmount = 0.00M)
        {
            var results = isArrearTransaction ? loanTransactionService.GetArrearTransactionsByParentFinancialServiceKey(financialServiceKey, date, transactionType)
                : loanTransactionService.GetFinancialTransactions(financialServiceKey, date, transactionType);
            Logger.LogAction(String.Format(@"Asserting that a transaction of type {0} exists against financial service {1} with a insert date
                                                greater than {2}", transactionType, financialServiceKey, date.ToString(Formats.DateTimeFormatSQL)));
            if (results.HasResults && transactionAmount > 0.00M)
            {
                var transactionExists = (from r in results where r.Column("Amount").GetValueAs<decimal>() == transactionAmount select r);
                Assert.That(transactionExists != null,
                    string.Format(@"Transaction {0} with value {1} does not exist against financial service {2}", (int)transactionType, transactionAmount, financialServiceKey));
            }
            else
            {
                Assert.True(results.HasResults,
                    string.Format(@"Transaction {0} does not exist against the financial service {1}.", (int)transactionType, financialServiceKey));
            }
        }

        /// <summary>
        /// This assertion will ensure that a financial transaction does not exist against the financial service provided.
        /// </summary>
        /// <param name="financialServiceKey">financialservicekey</param>
        /// <param name="date">transaction date</param>
        /// <param name="transactionType">transaction type</param>
        public static void AssertFinancialTransactionNotExists(int financialServiceKey, DateTime date, TransactionTypeEnum transactionType)
        {
            var results = loanTransactionService.GetFinancialTransactions(financialServiceKey, date, transactionType);
            Logger.LogAction(String.Format(@"Asserting that a financial transaction of type {0} has not been posted against financial service {1} with a insert date
                                                greater than {2}", transactionType, financialServiceKey, date.ToString(Formats.DateTimeFormatSQL)));
            Assert.False(results.HasResults, "Financial Transaction exists against the financial service, expected it to not exist.");
        }

        /// <summary>
        /// This assertion will check that all the transaction records for the specified financialservicetype have been deleted.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="financialServiceType"></param>
        public static void AssertFinancialTransactionsDeleted(int accountKey, FinancialServiceTypeEnum financialServiceType)
        {
            var results = loanTransactionService.GetFinancialTransactions(accountKey, financialServiceType);
            Assert.False(results.HasResults, "Financial Transactions still exists against accountkey: {0}, financialservicetype:{1}"
                    , accountKey, financialServiceType);
        }

        /// <summary>
        /// This assertion will check that all the arrear transaction records for the specified account and financialservicetype have been deleted.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="financialServiceType"></param>
        public static void AssertArrearTransactionsDeleted(int accountKey, FinancialServiceTypeEnum financialServiceType)
        {
            var results = loanTransactionService.GetArrearTransactions(accountKey, financialServiceType);
            Assert.False(results.HasResults, "Arrear Transactions still exists against accountkey: {0}, financialservicetype:{1} "
                        , accountKey, financialServiceType);
        }

        /// <summary>
        /// This assertion will check that a loan transaction record exists against on account.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="financialServiceType"></param>
        /// <param name="transactionType"></param>
        public static void AssertLoanTransactionExists(int accountKey, FinancialServiceTypeEnum financialServiceType, TransactionTypeEnum transactionType)
        {
            var rows = from r in loanTransactionService.GetFinancialTransactions(accountKey, financialServiceType)
                       where r.Column("TransactionTypeKey").GetValueAs<int>() == (int)transactionType
                       select r;

            Assert.True(rows.Count() > 0, "Loan Transaction does not exist against accountkey: {0}.", accountKey);
        }
    }
}