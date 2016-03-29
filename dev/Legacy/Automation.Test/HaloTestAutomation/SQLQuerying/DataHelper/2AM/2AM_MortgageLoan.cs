using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will retrieve a column value from the MortgageLoan table when provided with the AccountKey
        /// and the column to retrieve
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="columnName">Column to return</param>
        /// <returns>MortgageLoan.*</returns>
        public string GetMortgageLoanColumn(int accountKey, string columnName)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select {0} from [2am].[dbo].account a
					  join [2am].[dbo].financialservice fs on fs.accountkey=a.accountkey
					  join [2am].[fin].mortgageloan ml on ml.financialservicekey=fs.financialservicekey
                      join [2am].[fin].loanBalance lb on ml.financialServiceKey=lb.financialServiceKey
					  where a.accountkey={1}
                      and fs.accountstatuskey=1
					  and a.RRR_ProductKey in (1,2,5,6,9,11)",
                    columnName, accountKey);
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(columnName).Value;
        }

        /// <summary>
        /// Fetches the specified value from the Mortgage Loan table for the provided account.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="financialServiceTypeKey">financialServiceTypeKey</param>
        /// <param name="column">Column</param>
        /// <returns>MortgageLoan.*</returns>
        public string GetMortgageLoanColumn(int accountKey, int financialServiceTypeKey, string column)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(@"select lb.{0} from [2AM].[dbo].account a
								join [2AM].[dbo].financialService fs	on a.accountKey=fs.accountKey and fs.financialServiceTypeKey={1}
								join [2AM].[fin].mortgageLoan ml on fs.financialServiceKey=ml.financialServiceKey
                                join [2AM].[fin].LoanBalance lb on ml.FinancialServiceKey=lb.FinancialServiceKey
								where a.accountKey={2}", column, financialServiceTypeKey, accountKey);
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(column).Value;
        }

        /// <summary>
        /// Updates the remaining instalments for the Mortgage Loan
        /// </summary>
        /// <param name="accountKey">Account</param>
        /// <param name="term">New Term</param>
        public void UpdateRemainingTerm(int accountKey, int term)
        {
            string query = string.Format(@"update lb
                                set remainingInstalments = {0}
                                from [2am].[dbo].account a
                                join [2am].[dbo].financialservice fs on a.accountkey = fs.accountkey
                                join [2am].[fin].mortgageloan ml on fs.financialservicekey = ml.financialservicekey
                                join [2am].[fin].loanBalance lb on ml.FinancialServiceKey=lb.FinancialServiceKey
                                where a.accountkey = {1}
                                and fs.accountstatuskey=1", term, accountKey);

            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets the current balance for the mortgage loan account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns></returns>
        public double GetCurrentBalance(int accountKey)
        {
            string sql = string.Format(@"select * from [2AM].dbo.vMortgageLoanCurrentBalance where accountkey={0}", accountKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("CurrentBalance").GetValueAs<double>();
        }

        /// <summary>
        /// Gets the sum of the balances for the child financial services of the mortgage loan account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns></returns>
        public double GetSumOfChildFinancialServiceBalance(int accountKey)
        {
            string query =
               string.Format(@"select SUM(b.amount) as Balance from [2AM].dbo.financialservice fsp
                            join [2AM].dbo.financialservice fsc
	                            on fsp.financialservicekey=fsc.parentfinancialservicekey
                            join [2AM].fin.balance b
	                            on fsc.financialservicekey=b.financialservicekey
	                            and b.balancetypekey=1
                            where fsp.accountkey={0}
                            group by fsc.parentfinancialservicekey", accountKey);
            var statement = new SQLStatement { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column("Balance").GetValueAs<double>();
        }
    }
}