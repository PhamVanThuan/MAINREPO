using Common.Constants;
using Common.Enums;
using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get all the transactions for the accountkey that has an insert date that is bigger and where equals the transaction type
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="insertDate"></param>
        /// <param name="transactionTypeNumber"></param>
        /// <returns></returns>
        public QueryResults GetArrearTransactionByTypeDateAndAccountKey(int accountKey, string insertDate, int transactionTypeNumber)
        {
            //revamp query
            string query = string.Format(@"select at.* from [2am].dbo.financialservice fs (nolock)
                        join [2am].fin.arreartransaction at (nolock)
                        on fs.financialservicekey=at.financialservicekey
                        where fs.accountkey= {0}
                        and InsertDate > '{1}' and TransactionTypeKey = {2}
                        order by ArrearTransactionKey desc", accountKey, insertDate, transactionTypeNumber);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the value of the latest arrear transaction against an account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public QueryResults GetLatestArrearTransaction(int accountKey)
        {
            //revamp query
            string q = string.Format(@"select top 1 *, Balance, TransactionTypeKey
                            from [2am].dbo.FinancialService fs
                            join [2am].fin.ArrearTransaction at
                            on fs.FinancialServiceKey=at.FinancialServiceKey
                            where fs.accountkey={0}
                            order by ArrearTransactionKey desc", accountKey);
            //old query
            SQLStatement statement = new SQLStatement { StatementString = q };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r;
        }

        /// <summary>
        /// Back dates the effective date and actual effective date the specified number of days for an arrear transaction.
        /// </summary>
        /// <param name="arrearTransactionKey">arrearTransactionNumber</param>
        /// <param name="days">days</param>
        public void BackDateArrearTransaction(int arrearTransactionKey, int days)
        {
            string q = string.Format(@"update [2am].fin.arrearTransaction
                                    set EffectiveDate = cast(floor(cast(dateadd(dd,{1}, getdate()) as float))as datetime),
                                    CorrectionDate =  cast(floor(cast(dateadd(dd,{1}, getdate()) as float))as datetime)
                                    where arrearTransactionKey={0}", arrearTransactionKey, days);
            SQLStatement statement = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///   Returns a set of data for a specific loan transaction that was posted against a financialServiceKey after a given date.
        /// </summary>
        /// <param name = "financialServiceKey">financialServiceKey</param>
        /// <param name = "insertDate">LoanTransactionInsertDate</param>
        /// <param name = "transactionType">TransactionTypeNumber</param>
        /// <returns>LoanTransaction.*</returns>
        public QueryResults GetArrearTransactionsByParentFinancialServiceKey(int financialServiceKey, DateTime insertDate, TransactionTypeEnum transactionType)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select at.* from [2am].[dbo].financialService fs
                        join [2am].fin.ArrearTransaction at (nolock) on fs.financialserviceKey = at.financialServiceKey
                        where fs.parentFinancialServiceKey = {0} and fs.financialServiceTypeKey = 8
                        and at.InsertDate > '{1}' and at.TransactionTypeKey = {2}
                        order by at.ArrearTransactionKey desc",
                    financialServiceKey, insertDate.ToString(Formats.DateTimeFormatSQL), (int)transactionType);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Updates the balance value of an arrear transaction record
        /// </summary>
        /// <param name="arrearTransactionKey">arrearTransactionKey</param>
        /// <param name="amount">New Balance amount</param>
        public bool UpdateArrearTransactionBalance(int arrearTransactionKey, double amount)
        {
            var statement = new SQLStatement();
            var query = string.Format(@"Update fin.ArrearTransaction set Balance={0} where arrearTransactionKey = {1}", amount, arrearTransactionKey);
            statement.StatementString = query;
            return dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}