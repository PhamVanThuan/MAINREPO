using Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Deletes any FinancialServiceRecurringTransactions of a particular transaction type.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="transactionType"></param>
        public void DeleteManualDebitOrders(int accountKey)
        {
            string q =
                string.Format(@"delete from [2am].deb.ManualDebitOrder where
                                        financialServiceKey in (select financialServiceKey
                                        from [2am].dbo.FinancialService fs
                                        where fs.accountKey={0})", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        public bool UpdateFinancialServiceBankAccountStatus(int accountKey, GeneralStatusEnum generalStatus)
        {
            string q =
                string.Format(@"update [2am].dbo.financialServiceBankAccount set generalStatusKey={0}
                                where financialServiceKey in
                                (select fs.financialServiceKey from [2am].dbo.FinancialService fs where fs.accountkey={1})", (int)generalStatus, accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public void InsertFirstLegalEntityBankAccountAsFSBankAccount(int accountKey, FinancialServicePaymentTypeEnum paymentType)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.InsertFinancialServiceBankAccount" };
            proc.AddParameter(new SqlParameter("@accountKey", accountKey.ToString()));
            proc.AddParameter(new SqlParameter("@financialServicePaymentTypeKey", ((int)paymentType).ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Fetches the financial service recurring transactions for the specified account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>FinancialServiceRecurringTransaction Model</returns>
        public IEnumerable<Automation.DataModels.ManualDebitOrder> GetManualDebitOrders(int accountKey)
        {
            var manualDebitOrders = dataContext.Query<Automation.DataModels.ManualDebitOrder>(string.Format(
                @"select
                    mdo.manualDebitOrderKey,
                    isnull(bankAccountKey,0) as BankAccountKey,
                    TransactionTypeKey as TransactionType,
                    Reference,
                    InsertDate,
                    ActionDate,
                    isnull(m.Memo,'') as Notes,
                    mdo.GeneralStatusKey,
                    Amount,
                    UserID
                    from [2am].dbo.financialservice fs
                    join [2am].deb.manualDebitOrder mdo on fs.financialservicekey = mdo.financialservicekey
                    left join [2am].dbo.Memo m on mdo.memoKey = m.memoKey
                    where fs.accountkey={0}", accountKey));
            foreach (var mdo in manualDebitOrders)
            {
                if (mdo.BankAccountKey != 0)
                    mdo.BankAccount = (from b in GetBankAccount(mdo.BankAccountKey) select b).FirstOrDefault();
            }
            return manualDebitOrders;
        }

        /// <summary>
        /// Gets the mortgage loan financial services
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public List<Automation.DataModels.LoanFinancialService> GetLoanFinancialServices(int accountKey)
        {
            var sql = string.Format(@"select fs.financialServiceKey, fs.payment, ft.FinancialServiceTypeKey as FinancialServiceTypeKey,
                    AccountStatusKey, Term, InitialBalance, RemainingInstalments, InterestRate, ActiveMarketRate, RateAdjustment, MTDInterest,
                    CreditMatrixKey, m.Value as LinkRate, lb.ActiveMarketRate as MarketRate, ml.mortgageLoanPurposeKey as MortgageLoanPurpose
                    from [2am].dbo.financialService fs with (nolock)
                    join [2am].fin.loanBalance lb  with (nolock) on fs.financialServiceKey = lb.financialServiceKey
                    left join [2am].fin.mortgageLoan ml  with (nolock) on fs.financialServiceKey = ml.financialServiceKey
                    join [2am].dbo.financialServiceType ft  with (nolock) on fs.financialServiceTypeKey = ft.financialServiceTypeKey
	                    and ft.financialServiceGroupKey = 1
                    join [2am].dbo.rateConfiguration rc  with (nolock) on lb.rateConfigurationKey = rc.rateConfigurationKey
                    join [2am].dbo.margin m  with (nolock) on rc.marginkey = m.marginkey
                    where fs.accountkey = {0}", accountKey);
            var mlFS = dataContext.Query<Automation.DataModels.LoanFinancialService>(sql).ToList();
            foreach (var fs in mlFS)
            {
                fs.SuspendedInterest = GetSuspendedInterestBalanceByParentFinancialServiceKey(fs.FinancialServiceKey);
            }
            sql = string.Format(@"select * from dbo.FinancialService fs
                                    join fin.Balance b on fs.financialServiceKey = b.financialServiceKey
                                    where fs.accountKey = {0} and financialserviceTypeKey=8", accountKey);
            var arrearsFS = dataContext.Query<Automation.DataModels.LoanFinancialService>(sql).ToList();
            mlFS.AddRange(arrearsFS);
            return mlFS;
        }

        /// <summary>
        /// inserts a manual debit order
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="financialServiceKey"></param>
        /// <param name="bankAccountKey"></param>
        /// <returns></returns>
        public bool InsertManualDebitOrder(int financialServiceKey, int bankAccountKey, string userName)
        {
            dataContext.Execute(String.Format(
                   @"INSERT INTO [2AM].[deb].[ManualDebitOrder]
                          ( FinancialServiceKey,
                            InsertDate,
                            ActionDate,
                            TransactionTypeKey,
                            Reference,
                            Amount,
                            GeneralStatusKey,
                            UserID,
                            BankAccountKey,
                            MemoKey)
                          VALUES ('{0}', getdate(), dateadd(dd,+1,getdate()), 710, 'Test', 5000,  1, '{1}', {2}, null)",
                   financialServiceKey, userName, bankAccountKey));
            return true;
        }

        /// <summary>
        /// Fetches active debit order financial service bank accounts with the specified debit order day.
        /// </summary>
        /// <param name="debitOrderDay">Debit Order Day</param>
        /// <returns>Financial Service Bank Account Model</returns>
        public IEnumerable<Automation.DataModels.FinancialServiceBankAccountModel> GetDebitOrderFinancialServiceBankAccount(int debitOrderDay, bool isNaedoCompliant)
        {
            string sql = string.Format(@"select top 50 fsb.* from [2AM].dbo.financialservicebankaccount fsb
                                join [2AM].dbo.financialservice fs
	                                on fsb.financialservicekey=fs.financialservicekey
	                                and fs.accountstatuskey=1
	                                and fs.financialservicetypekey=1
                                left join [2AM].dbo.futuredatedchange fdc
	                                on fs.financialservicekey=fdc.identifierreferencekey
                                where fsb.financialservicepaymenttypekey=1
                                and fsb.generalstatuskey=1
                                and fsb.debitorderday={0}
                                and fsb.IsNaedoCompliant={1}
                                and fdc.futuredatedchangekey IS NULL
                                order by newid()", debitOrderDay, Convert.ToInt32(isNaedoCompliant));
            return dataContext.Query<Automation.DataModels.FinancialServiceBankAccountModel>(sql);
        }

        /// <summary>
        /// Gets the balance of the suspended interest financial service when provided with the mortgage loan parent fs key
        /// </summary>
        /// <param name="parentFinancialServiceKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Balance GetSuspendedInterestBalanceByParentFinancialServiceKey(int parentFinancialServiceKey)
        {
            string sql =
                string.Format(@"select fs.financialServiceKey, b.Amount from [2am].dbo.account a
                    join [2am].dbo.financialService fs on a.accountKey = fs.accountKey
	                    and fs.financialservicetypekey=7
                    join [2am].fin.balance b on fs.financialservicekey = b.financialservicekey
                    where fs.parentfinancialservicekey={0} and fs.accountstatuskey = 1 ", parentFinancialServiceKey);
            return dataContext.Query<Automation.DataModels.Balance>(sql).FirstOrDefault();
        }

        /// <summary>
        /// Fetches the financial service bank accounts for the account provided.
        /// </summary>
        /// <param name="accountKey">Account</param>
        public IEnumerable<Automation.DataModels.FinancialServiceBankAccountModel> GetFinancialServiceBankAccounts(int accountKey)
        {
            string sql = string.Format(@"select fsba.* from [2am].dbo.account a with (nolock)
                join [2am].dbo.financialservice fs with (nolock) on a.accountkey = fs.accountkey
	                and fs.parentFinancialServiceKey is null
                join [2am].dbo.financialServiceBankAccount fsba with (nolock)
                on fs.financialServiceKey = fsba.financialservicekey
                where a.accountkey={0}", accountKey);
            return dataContext.Query<Automation.DataModels.FinancialServiceBankAccountModel>(sql);
        }

        /// <summary>
        /// Fetches the DOTransaction record for an account within an active Neado tracking period.
        /// </summary>
        public IEnumerable<Automation.DataModels.DOTransaction> GetAccountWithinNaedoTrackingPeriod()
        {
            string query = string.Format(
                    @"select top 1 dt.*
                        from [2AM].deb.Batch b
                        join [2AM].deb.DOTransaction dt on b.BatchKey = dt.BatchKey
                        where b.ProviderKey = 3
                        and b.TransactionTypeKey = 310
                        and dt.TransactionStatusKey in (2, 3)
                        and isnull(dt.ErrorKey, 128) = 128
                        order by newid()");
            return dataContext.Query<Automation.DataModels.DOTransaction>(query);
        }

        /// <summary>
        /// Updates the AccountKey for the given DOTransaction record.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="DOTransactionKey"></param>        
        public void UpdateAccountOnDOTransactionRecord(int accountKey, int DOTransactionKey)
        {
            string q =
                string.Format(@"update [2AM].deb.DOTransaction 
                                set AccountKey = {0}
                                where DOTransactionKey = {1}", accountKey, DOTransactionKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}