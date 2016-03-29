using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   This will retrieve the disbursement records of a particular type linked to an account.
        /// </summary>
        /// <param name = "accountKey">AccountKey</param>
        /// <param name = "disbType">DisbursementTransactionType.Description i.e. CAP2 ReAdvance</param>
        /// <param name="rows">No of rows to fetch</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetDisbursementFinancialTransactionByAccountKeyAndDisbursementType(int accountKey, string disbType, string rows)
        {
            string query =
                string.Format(
                    @"select top {0} *
                        from [2am].dbo.disbursement d with (nolock)
                        join [2am].dbo.disbursementTransactionType dtt with (nolock)
                        on d.disbursementtransactiontypekey=dtt.disbursementtransactiontypekey
                        left join [2am].dbo.disbursementFinancialTransaction dft with (nolock) on d.disbursementkey=dft.disbursementkey
                        left join [2am].fin.FinancialTransaction ft with (nolock) on dft.FinancialTransactionKey=ft.FinancialTransactionKey
                        where accountkey= {1}
                        and dtt.description='{2}'
                        order by ft.InsertDate desc",
                    rows, accountKey, disbType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns disbursement records at a given status when provided with an account and a Disbursement Transaction Type
        /// </summary>
        /// <param name = "accountKey">Account</param>
        /// <param name = "disbursementStatusKey">Disbursement Status</param>
        /// <param name = "disbTranTypeKey">Disbursement Transaction Type</param>
        /// <returns>Disbursement.*</returns>
        public QueryResults GetDisbursementRecords(int accountKey, int disbursementStatusKey, int disbTranTypeKey)
        {
            SQLStatement statement = new SQLStatement();
            string query =
                String.Format(
                    @"select * from [2am].dbo.Disbursement (nolock)
                    where AccountKey = {0}
                        and DisbursementStatusKey = {1}
                        and DisbursementTransactionTypeKey = {2}
                    order by 1 desc",
                    accountKey, disbursementStatusKey, disbTranTypeKey);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Disbursement> GetDisbursementRecordsForOpenAccounts()
        {
            var sql = string.Format(@"select d.*, br.*, b.*, t.*
					from [2am].dbo.Account a
						join [2am].dbo.Disbursement d on a.AccountKey = d.AccountKey
						join [2am].dbo.acbbank b on d.acbbankcode = b.acbbankcode
						join [2am].dbo.acbbranch br on d.acbbranchcode = br.acbbranchcode
						join [2am].dbo.acbtype t on d.acbtypenumber = t.acbtypenumber
					where a.AccountStatusKey = 1
                        and d.PreparedDate > '2011/01/01'");
            //var results = cn.Query<Models.DisbursementModel, Models.BankAccountModel, Models.DisbursementModel>(sql, (d, b) => { d.BankAccount = b; return d; }, splitOn: "ACBBranchCode");
            var results = dataContext.Query<Automation.DataModels.Disbursement>(sql);
            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Disbursement> GetDisbursementRecordsWithLoanTransactionsForOpenAccounts()
        {
            var sql = @"select d.*, dt.*, ds.*, dlt.*, lt.*
                from [2am].dbo.Account a
                join [2am].dbo.Disbursement d on a.AccountKey = d.AccountKey
                join [2am].dbo.DisbursementtransactionType dt on d.DisbursementTransactionTypeKey = dt.DisbursementTransactionTypeKey
                join [2am].dbo.DisbursementStatus ds on d.DisbursementStatusKey = ds.DisbursementStatusKey
                join [2am].dbo.DisbursementFinancialTransaction dlt on d.DisbursementKey = dlt.DisbursementKey
                join [2am].fin.financialTransaction lt on dlt.FinancialTransactionKey = lt.FinancialTransactionKey
                where a.AccountStatusKey = 1
                and d.PreparedDate > '2011/01/01'";
            var results = dataContext.Query<
                Automation.DataModels.Disbursement,
                Automation.DataModels.DisbursementTransactionType,
                Automation.DataModels.DisbursementStatus,
                Automation.DataModels.DisbursementFinancialTransaction,
                Automation.DataModels.LoanTransaction,
                Automation.DataModels.Disbursement
                >(
                sql,
                (d, dt, ds, dlt, lt) =>
                {
                    d.DisbursementTransactionType = dt;
                    d.DisbursementStatus = ds;
                    d.DisbursementLoanTransaction = dlt;
                    dlt.LoanTransaction = lt;
                    return d;
                },
                splitOn: "DisbursementTransactionTypeKey,DisbursementStatusKey,DisbursementKey,FinancialTransactionKey",
                commandtype: System.Data.CommandType.Text
                );

            return results;
        }

        public bool UpdateDisbursementStatusForAccount(int accountKey, DisbursementStatusEnum currentStatus, DisbursementStatusEnum newStatus)
        {
            SQLStatement statement = new SQLStatement();
            string query =
                String.Format(
                    @"update [2am].dbo.Disbursement
                    set DisbursementStatusKey = {0}
                    where AccountKey = {1} and DisbursementStatusKey = {2}", (int)newStatus, accountKey, (int)currentStatus);
            statement.StatementString = query;
            return dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}