using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Exceptions;

namespace BuildingBlocks.Services
{
    public class LoanTransactionService : _2AMDataHelper, ILoanTransactionService
    {
        /// <summary>
        /// Gets a Loan Transaction record
        /// </summary>
        /// <param name="columnToGet">column from the loan transaction table to retrieve</param>
        /// <param name="columnToRetrieveBy">The Column for the WHERE clause</param>
        /// <param name="columnValue">The value of the column in the WHERE clause</param>
        /// <returns></returns>
        public string GetLoanTransactionColumn(string columnToGet, string columnToRetrieveBy, string columnValue)
        {
            var results = base.GetFinancialTransaction(columnToRetrieveBy, columnValue);
            if (!results.HasResults)
                throw new WatiNException(string.Format(@"No Loan Transaction Record Found: Retrieving: {0}, ByColumn: {1}, ColumnValue: {2}",
                    columnToGet, columnToRetrieveBy, columnValue));
            return results.Rows(0).Column(columnToGet).Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public Dictionary<int, double> GetLatestArrearTransaction(int accountKey)
        {
            var results = base.GetLatestArrearTransaction(accountKey);
            var dic = new Dictionary<int, double>
                {
                    {
                        results.Rows(0).Column("TransactionTypeKey").GetValueAs<int>(),
                        results.Rows(0).Column("Balance").GetValueAs<double>()
                    }
                };
            return dic;
        }

        /// <summary>
        /// Fetches the latest arrear transaction key for the specified account.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        public int GetLatestArrearTransactionKey(int accountKey)
        {
            int arrearTransactionKey = (from r in base.GetLatestArrearTransaction(accountKey)
                                        select r.Column("ArrearTransactionKey").GetValueAs<int>()).FirstOrDefault();
            return arrearTransactionKey;
        }

        /// <summary>
        /// Fetches the latest arrear balance amount for the specified account.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns></returns>
        public decimal GetLatestArrearBalanceAmount(int accountKey)
        {
            decimal balance = (from r in base.GetLatestArrearTransaction(accountKey)
                               select r.Column("Balance").GetValueAs<decimal>()).FirstOrDefault();
            return balance;
        }

        /// <summary>
        /// Gets the transaction types that an AD User Group has access to.
        /// </summary>
        /// <param name="adcredentials"></param>
        /// <param name="transactionTypes"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.TransactionType> GetTransactionDataAccess(string adcredentials, params int[] transactionTypes)
        {
            var transactiontypes = base.GetManyTransactionDataAccessRecords(transactionTypes);
            return (from tt in transactiontypes
                    where tt.ADCredentials == adcredentials
                    select tt);
        }

        /// <summary>
        /// Fetches the remaining instalments for the specified account.
        /// </summary>
        /// <param name="financialServiceKey">accountKey</param>
        public int GetRemainingInstalmentsOnLoan(int financialServiceKey)
        {
            int numberOfInstalments = (from r in base.GetRemainingInstalmentsOnLoan(financialServiceKey)
                                        select r.Column("RemainingInstalments").GetValueAs<int>()).FirstOrDefault();
            return numberOfInstalments;
        }


        public void UpdateRemainingInstalmentsOnLoan(int financialServiceKey, int numberOfInstalments)
        {
            base.UpdateRemainingInstalmentsOnLoan(financialServiceKey, numberOfInstalments);
        }
    }
}