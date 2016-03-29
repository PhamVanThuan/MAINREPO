using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   Returns the loan transactions linked to a given DisbursementKey via the DisbursementFinancialTransaction linking table
        /// </summary>
        /// <param name = "disbursementKey">DisbursementKey</param>
        /// <returns>LoanTransaction.*</returns>
        public QueryResults GetLoanTransactionRecordsByDisbursementKey(int disbursementKey)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select ft.* from [2am].dbo.DisbursementFinancialTransaction dft (nolock)
                        join [2am].fin.FinancialTransaction ft (nolock)
                        on dft.FinancialTransactionKey=ft.FinancialTransactionKey
                        where DisbursementKey = {0}",
                    disbursementKey);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns a set of data for a specific loan transaction that was posted against an account after a given date.
        /// </summary>
        /// <param name = "accountKey">AccountKey</param>
        /// <param name = "insertDate">LoanTransactionInsertDate</param>
        /// <param name = "transactionTypeNumber">TransactionTypeNumber</param>
        /// <returns>LoanTransaction.*</returns>
        public QueryResults GetLoanTransactionByTypeDateAndAccountKey(int accountKey, DateTime insertDate, int transactionTypeNumber)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select ft.* from [2am].dbo.financialservice fs (nolock)
                    join [2am].fin.financialTransaction ft (nolock) on fs.financialservicekey=ft.financialservicekey
                    where fs.accountkey = {0}
                    and ft.InsertDate > '{1}'
                    and ft.TransactionTypeKey = {2}
                    order by ft.FinancialTransactionKey desc",
                    accountKey, insertDate.ToString(Formats.DateTimeFormatSQL), transactionTypeNumber);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets a loan transaction record
        /// </summary>
        /// <param name="column">Column to query by</param>
        /// <param name="value">Column value to query by</param>
        /// <returns></returns>
        public QueryResults GetFinancialTransaction(string column, string value)
        {
            string query =
                String.Format(
                            @"SELECT top 1 * FROM [2am].fin.FinancialTransaction (nolock) WHERE {0} = '{1}' ORDER BY 1 DESC ",
                            column, value);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Posts a financial transaction against a financial service
        /// </summary>
        /// <param name="financialServiceKey">FinancialServiceKey</param>
        /// <param name="transactionType">transactionType</param>
        /// <param name="amount">transactionAmount</param>
        /// <param name="reference">transactionReference</param>
        /// <param name="userID">userID</param>
        public void pProcessTran(int financialServiceKey, TransactionTypeEnum transactionType, decimal amount, string reference, string userID)
        {
            string query =
                string.Format(@"BEGIN TRAN ProcessTran

                                DECLARE @Msg varchar(1024)
                                DECLARE @Ret int

                                EXECUTE @Ret = [process].[halo].[pProcessTran]
                                {0}, {1}, {2}, '{3}', '{4}', @Msg OUTPUT

                                IF @Ret <> 0 OR ISNULL(@Msg, '') <> ''
	                                BEGIN
		                                ROLLBACK TRAN ProcessTran
	                                END
                                ELSE
	                                BEGIN
		                                COMMIT TRAN ProcessTran
	                                END", financialServiceKey, (int)transactionType, amount, reference, userID);
            SQLStatement statement = new SQLStatement { StatementString = query };
            bool success = dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transactionTypes"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.TransactionType> GetManyTransactionDataAccessRecords(params int[] transactionTypes)
        {
            var transactionTypeNoStr = Helpers.GetDelimitedString<int>(transactionTypes, ",");
            return
                dataContext.Query<Automation.DataModels.TransactionType>(
                    String.Format(
                        @"select * from dbo.transactiontypedataaccess ttda
                            inner join fin.transactiontype tt
                            on ttda.transactiontypekey = tt.transactiontypekey
                            inner join dbo.transactiontypeUI ui on tt.transactiontypekey = ui.transactiontypekey
                            where ttda.transactiontypekey in ({0})",
                        transactionTypeNoStr));
        }

        /// <summary>
        ///   Returns a set of data for a specific loan transaction that was posted against a financialServiceKey after a given date.
        /// </summary>
        /// <param name = "financialServiceKey">financialServiceKey</param>
        /// <param name = "insertDate">LoanTransactionInsertDate</param>
        /// <param name = "transactionTypeKey">TransactionTypeNumber</param>
        /// <returns>LoanTransaction.*</returns>
        public QueryResults GetFinancialTransactions(int financialServiceKey, DateTime insertDate, TransactionTypeEnum transactionType)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select ft.* from [2am].fin.financialTransaction ft (nolock)
                    where ft.financialServiceKey = {0} and ft.InsertDate > '{1}'
                    and ft.TransactionTypeKey = {2} order by ft.FinancialTransactionKey desc",
                    financialServiceKey, insertDate.ToString(Formats.DateTimeFormatSQL), (int)transactionType);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Posts a payment transaction against the required financial services.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="transactionType">Transaction Type</param>
        /// <param name="amount">Amount</param>
        /// <param name="reference">Reference</param>
        /// <param name="userID">User ID</param>
        /// <param name="effectiveDate">Effective Date</param>
        public QueryResults pProcessAccountPaymentTran(int accountKey, TransactionTypeEnum transactionType, double amount, string reference,
            string userID, DateTime effectiveDate)
        {
            string query =
                string.Format(@"BEGIN TRAN ProcessTran

                                DECLARE @Msg varchar(1024)
                                DECLARE @Ret int

                                EXECUTE @Ret = [process].[halo].[pProcessAccountPaymentTran]
                                {0}, {1}, {2}, '{3}', '{4}', '{5}', @Msg OUTPUT

                                IF @Ret <> 0 OR ISNULL(@Msg, '') <> ''
	                                BEGIN
		                                ROLLBACK TRAN ProcessTran
	                                END
                                ELSE
	                                BEGIN
		                                COMMIT TRAN ProcessTran
	                                END", (int)accountKey, (int)transactionType, amount, reference, userID, effectiveDate.ToString(Formats.DateTimeFormatSQL));
            SQLStatement statement = new SQLStatement { StatementString = query };
            bool success = dataContext.ExecuteNonSQLQuery(statement);
            return GetLoanTransactionByTypeDateAndAccountKey(accountKey, DateTime.Now.AddMinutes(-1), (int)transactionType);
        }

        /// <summary>
        /// Returns TRUE if a transaction is an arrears transaction or FALSE if it is not.
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public bool IsTransactionAnArrearTransaction(TransactionTypeEnum transactionType)
        {
            string sql = string.Format(@"select * from [2am].[fin].TransactionType tt (nolock)
                    join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
                    join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                    where tt.TransactionTypeKey = {0} and tte1.transactionEffectKey < 4 and balanceTypeKey = 4", (int)transactionType);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.HasResults;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public QueryResults GetFinancialTransactionForRollback(ProductEnum product)
        {
            string sql =
                string.Format(@"SELECT TOP 10
                Fs.AccountKey,
                FT.FinancialTransactionKey,
                FT.TransactionTypeKey,
                FT.financialServiceKey
                FROM
                [2AM].[fin].FinancialTransaction FT (NOLOCK)
                INNER JOIN [2AM].[fin].[TransactionType] TT (NOLOCK)
                ON FT.TransactionTypeKey = TT.TransactionTypeKey
                INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK)
                ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                INNER JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK)
                ON FT.FinancialServiceKey = FS.FinancialServiceKey
                    AND FS.parentFinancialServiceKey is null
                INNER JOIN [2AM].[dbo].Account a (NOLOCK) on FS.AccountKey = a.AccountKey
                LEFT JOIN
                (	SELECT DISTINCT TT.TransactionTypeKey
                    FROM [2AM].[fin].[TransactionType] TT (NOLOCK)
                    INNER JOIN [2AM].[dbo].TransactionTypeUI TTUI (NOLOCK)
                    ON TT.TransactionTypeKey = TTUI.TransactionTypeKey
                        AND TTUI.ScreenBatch = 1
                    INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK)
                    ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                        AND TTG.TransactionGroupKey = 1
                        AND TTG.TransactionTypeKey NOT IN
                        (SELECT TransactionTypeKey FROM [2am].fin.TransactionTypeGroup (NOLOCK) WHERE TransactionGroupKey = 3)
                 INNER JOIN [2AM].[dbo].[TransactionTypeDataAccess] TTDA (NOLOCK) ON TTDA.TransactionTypeKey = TT.TransactionTypeKey
                 WHERE TTDA.ADCredentials IN
                 ( SELECT item FROM [2AM].dbo.fSplitDelimitedList('ITStaff', ','))) CanRollBack
                 ON FT.TransactionTypeKey = CanRollBack.TransactionTypeKey
                WHERE
                CanRollBack.TransactionTypeKey IS NOT NULL
                AND FT.IsRolledBack = 0  AND TT.ReversalTransactionTypeKey IS NOT NULL
                AND (TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2)
                AND a.rrr_ProductKey = {0} AND a.AccountStatusKey = 1
                ORDER BY ft.FinancialTransactionKey DESC", (int)product
                );
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the transaction type details
        /// </summary>
        /// <param name="transactionTypeKey"></param>
        /// <returns></returns>
        public Automation.DataModels.TransactionType GetTransactionTypeDetails(int transactionTypeKey)
        {
            var sql = string.Format(@"select transactionTypeKey, reversalTransactionTypeKey, Description
                        from [2am].fin.transactiontype  where transactiontypekey={0}", transactionTypeKey);
            var transactionType = dataContext.Query<Automation.DataModels.TransactionType>(sql);
            return (from t in transactionType select t).FirstOrDefault();
        }

        /// <summary>
        /// Gets an arrear transaction that can be rolled back
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public QueryResults GetArrearTransactionForRollback(ProductEnum product)
        {
            string sql = string.Format(@"SELECT TOP 10
                            Fs.AccountKey,
                            FT.ArrearTransactionKey,
                            FT.TransactionTypeKey,
                            FT.financialServiceKey
                            FROM
                            [2AM].[fin].ArrearTransaction FT (NOLOCK)
                            INNER JOIN [2AM].[fin].[TransactionType] TT (NOLOCK)
                            ON FT.TransactionTypeKey = TT.TransactionTypeKey
                            INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK)
                            ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                            INNER JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK)
                            ON FT.FinancialServiceKey = FS.FinancialServiceKey
                            INNER JOIN [2AM].[dbo].Account a (NOLOCK) on FS.AccountKey = a.AccountKey
                            LEFT JOIN
                            (	SELECT DISTINCT TT.TransactionTypeKey
                                FROM [2AM].[fin].[TransactionType] TT (NOLOCK)
                                INNER JOIN [2AM].[dbo].TransactionTypeUI TTUI (NOLOCK)
                                ON TT.TransactionTypeKey = TTUI.TransactionTypeKey
                                    AND TTUI.ScreenBatch = 1
                                INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK)
                                ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                                    AND TTG.TransactionGroupKey = 1
                                    AND TTG.TransactionTypeKey NOT IN
                                    (SELECT TransactionTypeKey FROM [2am].fin.TransactionTypeGroup (NOLOCK) WHERE TransactionGroupKey = 3)
                             INNER JOIN [2AM].[dbo].[TransactionTypeDataAccess] TTDA (NOLOCK)
                             ON TTDA.TransactionTypeKey = TT.TransactionTypeKey
                             WHERE TTDA.ADCredentials IN
                             ( SELECT item FROM [2AM].dbo.fSplitDelimitedList('ITStaff', ','))) CanRollBack
                             ON FT.TransactionTypeKey = CanRollBack.TransactionTypeKey
                            WHERE
                            CanRollBack.TransactionTypeKey IS NOT NULL
                            AND FT.IsRolledBack = 0  AND TT.ReversalTransactionTypeKey IS NOT NULL
                            AND (TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2)
                            AND a.rrr_ProductKey = {0} AND a.AccountStatusKey = 1
                            ORDER BY ft.ArrearTransactionKey DESC", (int)product);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets an account that has the transaction type provided posted against one of its financial services.
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public QueryResults GetAccountWithLoanTransactionAgainstParentFinancialService(TransactionTypeEnum transactionType)
        {
            string sql = string.Format(@"select top 10 a.accountKey, ft.financialTransactionKey, ft.financialServiceKey
                        from [2am].[dbo].account a (nolock)
                        left join Role r on a.AccountKey = r.AccountKey and r.RoleTypeKey = 3
                        join [2am].[dbo].financialService fs (nolock) on a.accountKey = fs.accountKey
                            and fs.parentFinancialServiceKey is null
                        join [2am].[fin].financialTransaction ft (nolock) on fs.financialServiceKey = ft.financialServiceKey
                        where ft.transactiontypekey = {0}
                            and r.AccountRoleKey is null
                        order by ft.financialTransactionKey desc", (int)transactionType);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///  Returns all the financialtransactions for a specific account.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns>all the columns of dbo.financialtransaction</returns>
        public QueryResults GetFinancialTransactions(int accountkey, FinancialServiceTypeEnum financialServiceType)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select ft.* from [2am].fin.financialTransaction ft (nolock)
	                        inner join [2am].dbo.financialservice as fs (nolock)
		                        on ft.financialservicekey = fs.financialservicekey
                        where fs.accountkey = {0} and fs.financialservicetypekey = {1}
                        order by ft.FinancialTransactionKey desc", accountkey, (int)financialServiceType);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns all the arreartransactions for a specific account.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns>all the columns of dbo.arreartransaction</returns>
        public QueryResults GetArrearTransactions(int accountkey, FinancialServiceTypeEnum financialServiceType)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select at.* from [2am].fin.ArrearTransaction as at (nolock)
	                        inner join [2am].dbo.financialservice as fs (nolock)
		                        on at.financialservicekey = fs.financialservicekey
                        where fs.accountkey = {0} and fs.financialservicetypekey = {1}",
                                                        accountkey, (int)financialServiceType);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}