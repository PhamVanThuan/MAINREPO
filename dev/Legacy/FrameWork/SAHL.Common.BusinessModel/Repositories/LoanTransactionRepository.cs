using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DataSets;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ILoanTransactionRepository))]
    public class LoanTransactionRepository : AbstractRepositoryBase, ILoanTransactionRepository
    {
        public LoanTransactionRepository()
        {
            if (castleTransactionsService == null)
            {
                castleTransactionsService = new CastleTransactionsService();
            }
        }

        public LoanTransactionRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionsService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionsService;

        public IFinancialTransaction GetLoanTransactionByLoanTransactionNumber(int LoanTransactionNumber)
        {
            return base.GetByKey<IFinancialTransaction, FinancialTransaction_DAO>(LoanTransactionNumber);
        }

        /// <summary>
        /// Return the most recent transaction for the financial service by type
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="financialServiceKey"></param>
        /// <param name="includeRolledbackTransactions"></param>
        /// <returns></returns>
        public IFinancialTransaction GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(TransactionTypes transactionType, int financialServiceKey, bool includeRolledbackTransactions)
        {
            string hql = "";
            if (includeRolledbackTransactions)
                hql = "from FinancialTransaction_DAO lt where lt.FinancialService.Key = ? and lt.TransactionType.Key = ? order by lt.InsertDate desc";
            else
                hql = "from FinancialTransaction_DAO lt where lt.FinancialService.Key = ? and lt.TransactionType.Key = ? and (lt.IsRolledBack = '0') order by lt.InsertDate desc";
            SimpleQuery<FinancialTransaction_DAO> query = new SimpleQuery<FinancialTransaction_DAO>(hql, financialServiceKey, (int)transactionType);
            query.SetQueryRange(1);

            FinancialTransaction_DAO[] lt = query.Execute();

            if (lt.Length > 0)
                return new FinancialTransaction(lt[0]);

            return null;
        }

        /// <summary>
        /// Return the most recent transaction for the Account by type
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="accountKey"></param>
        /// <param name="includeRolledbackTransactions"></param>
        /// <returns></returns>
        public IFinancialTransaction GetLastLoanTransactionByTransactionTypeAndAccountKey(TransactionTypes transactionType, int accountKey, bool includeRolledbackTransactions)
        {
            string hql = "";
            if (includeRolledbackTransactions)
                hql = "from FinancialTransaction_DAO lt where lt.FinancialService.Account.Key = ? and lt.TransactionType.Key = ? order by lt.InsertDate desc";
            else
                hql = "from FinancialTransaction_DAO lt where lt.FinancialService.Account.Key = ? and lt.TransactionType.Key = ? and (lt.IsRolledBack = '0') order by lt.InsertDate desc";
            SimpleQuery<FinancialTransaction_DAO> query = new SimpleQuery<FinancialTransaction_DAO>(hql, accountKey, (int)transactionType);
            query.SetQueryRange(1);

            FinancialTransaction_DAO[] lt = query.Execute();

            if (lt.Length > 0)
                return new FinancialTransaction(lt[0]);

            return null;
        }

        /// <summary>
        /// Pass account key into CallProcessAccountPaymentTran
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionTypeKey"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionAmount"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="ADUserName"></param>
        public void PostTransactionByAccountKey(int AccountKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, double TransactionAmount, string TransactionReference, string ADUserName)
        {
            string CallingApp = string.Empty;
            IAccountRepository AccRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = AccRepo.GetAccountByKey(AccountKey);
            PostTransactionValidation(account, TransactionTypeKey, TransactionEffectiveDate, TransactionAmount);
            ProcessAccountPaymentTran(AccountKey, TransactionTypeKey, TransactionAmount, TransactionReference, ADUserName, TransactionEffectiveDate);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <param name="TransactionTypeKey"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionAmount"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="ADUserName"></param>
        public void PostTransactionByFinancialServiceKey(int FinancialServiceKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, double TransactionAmount, string TransactionReference, string ADUserName)
        {
            string CallingApp = string.Empty;

            IFinancialServiceRepository finRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFinancialService financialService = finRepo.GetFinancialServiceByKey(FinancialServiceKey);
            PostTransactionValidation(financialService.Account, TransactionTypeKey, TransactionEffectiveDate, TransactionAmount);
            pProcessTran(FinancialServiceKey, TransactionTypeKey, TransactionEffectiveDate, TransactionAmount, TransactionReference, ADUserName);
        }

        private void PostTransactionValidation(IAccount account, int TransactionTypeKey, DateTime TransactionEffectiveDate, double TransactionAmount)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            switch (TransactionTypeKey)
            {
                case -1:
                    {
                        string errorMessage = "Please select a Transaction Type";
                        dmc.Add(new Error(errorMessage, errorMessage));
                        break;
                    }

                case (int)TransactionTypes.DebitNonPerformingInterest:
                    {
                        svc.ExecuteRule(dmc, "PostTransactionNonPerformingLoan236_966", account);
                        svc.ExecuteRule(dmc, "PostTransactionNonPerformingLoan236_967", account, TransactionAmount);
                        break;
                    }
                case (int)TransactionTypes.NonPerformingInterest:
                    {
                        svc.ExecuteRule(dmc, "PostTransactionNonPerformingLoan236_966", account);
                        break;
                    }
                case (int)TransactionTypes.ReverseNonPerformingInterest:
                    {
                        svc.ExecuteRule(dmc, "PostTransactionNonPerformingLoan967", account);
                        svc.ExecuteRule(dmc, "PostTransactionNonPerformingLoan236_967", account, TransactionAmount);
                        break;
                    }
            }

            if (TransactionAmount <= 0)
            {
                string errorMessage = "Please enter an amount greater than zero";
                dmc.Add(new Error(errorMessage, errorMessage));
            }

            if (TransactionEffectiveDate == new DateTime())
            {
                string errorMessage = "Please select a valid Effective Date";
                dmc.Add(new Error(errorMessage, errorMessage));
            }

            svc.ExecuteRule(dmc, "PostTransactionCheckDateLessThanFirstOfCurrentMonth", TransactionEffectiveDate);
            svc.ExecuteRule(dmc, "PostTransactionCheckEffectiveDate", TransactionEffectiveDate, TransactionTypeKey);

            if (dmc.Count > 0)
                throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="StatementName"></param>
        /// <param name="ApplicationName"></param>
        /// <param name="Parameters"></param>
        /// <param name="FetchRows"></param>
        /// <returns></returns>
        public DataTable GetTransactions(string StatementName, string ApplicationName, List<SqlParameter> Parameters, int FetchRows)
        {
            string sql = UIStatementRepository.GetStatement("COMMON", StatementName);

            string query = String.Format(sql, FetchRows.ToString());

            ParameterCollection prms = new ParameterCollection();
            foreach (SqlParameter prm in Parameters)
            {
                prms.Add(prm);
            }

            DataTable DT = new DataTable();
            DataSet DS = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
            if (DS.Tables != null && DS.Tables.Count > 0)
                DT = DS.Tables[0];

            return DT;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionNumber"></param>
        /// <param name="UserID"></param>
        /// <param name="IsArrear"></param>
        public void RollbackTransaction(int AccountKey, int TransactionNumber, string UserID, bool IsArrear)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            ILoanTransactionRepository loanTransactionRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
            IAccount account;
            IFinancialTransaction loanTransaction = loanTransactionRepo.GetLoanTransactionByLoanTransactionNumber(TransactionNumber);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            switch (loanTransaction.TransactionType.Key)
            {
                case 236:
                    {
                        account = accRepo.GetAccountByKey(AccountKey);
                        svc.ExecuteRule(dmc, "RollbackTransactionNonPerformingLoan236", account);
                        break;
                    }
            }

            if (dmc.Count == 0)
            {
                ParameterCollection prms = new ParameterCollection();

                if (IsArrear)
                {
                    prms.Add(new SqlParameter("@UserID", UserID));
                    prms.Add(new SqlParameter("@ArrearTransactionKey", TransactionNumber));
                    this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "SP_pLoanProcessRollbackArrearTran", prms);
                }
                else
                {
                    prms.Add(new SqlParameter("@UserID", UserID));
                    prms.Add(new SqlParameter("@FinancialTransactionKey", TransactionNumber));
                    this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "SP_pLoanProcessRollbackTran", prms);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetAdvancesDisbursedThisYearByAccountKey(int key)
        {
            string sql = UIStatementRepository.GetStatement("COMMON", "AdvancesThisYearGetByAccountKey");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", key));

            DataTable DT = new DataTable();
            DataSet DS = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);
            if (DS.Tables != null && DS.Tables.Count > 0)
                DT = DS.Tables[0];

            return DT.Rows.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialServiceKey"></param>
        /// <param name="transactionTypeKey"></param>
        /// <param name="amount"></param>
        /// <param name="reference"></param>
        /// <param name="userID"></param>
        /// <returns>Messages in the Domain Message Collection for any failure.</returns>
        public void pProcessTran(int financialServiceKey, int transactionTypeKey, DateTime effectiveDate, double amount, string reference, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", financialServiceKey));
            prms.Add(new SqlParameter("@TransactionTypeKey", transactionTypeKey));
            prms.Add(new SqlParameter("@EffectiveDate", effectiveDate));
            prms.Add(new SqlParameter("@Amount", amount));
            prms.Add(new SqlParameter("@Reference", reference));
            prms.Add(new SqlParameter("@UserId", userID));

            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("FinancialService", "pProcessTranWithEffectiveDate", prms);
        }

        public void pLoanProcessRollbackTran(int LoanTransactionNumber, SAHLPrincipal principal)
        {
            string UserID = principal.Identity.Name;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialTransactionKey", LoanTransactionNumber));
            prms.Add(new SqlParameter("@UserID", UserID));
            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "SP_pLoanProcessRollbackTran", prms);
        }

        public void ProcessAccountPaymentTran(int accountKey, int transactionTypeKey, double amount, string reference, string UserID, DateTime effectivedate)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@TransactionTypeKey", transactionTypeKey));
            prms.Add(new SqlParameter("@Amount", amount));
            prms.Add(new SqlParameter("@Reference", reference));
            prms.Add(new SqlParameter("@UserID", UserID));
            prms.Add(new SqlParameter("@Effectivedate", effectivedate));
            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "ProcessAccountPaymentTran", prms);
        }

        /// <summary>
        /// Executes the uistatement of the same name
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionTypeKey"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="UserID"></param>
        /// <returns>The loantransactionnumber if found else -1</returns>
        public int FindLoanProcessTran(int AccountKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, string TransactionReference, string UserID)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "FindLoanProcessTran");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@l_LoanNumber", AccountKey));
            prms.Add(new SqlParameter("@i_TransactionTypeNumber", TransactionTypeKey));
            prms.Add(new SqlParameter("@s_TransactionEffectiveDate", TransactionEffectiveDate));
            prms.Add(new SqlParameter("@s_TransactionReference", TransactionReference));
            prms.Add(new SqlParameter("@s_UserId", UserID));

            object obj = this.castleTransactionsService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (obj != DBNull.Value && obj != null)
            {
                int loanTransactionNumber = Convert.ToInt32(obj);
                return loanTransactionNumber;
            }

            return -1;
        }

        public DataTable FindLoanProcessTran(int AccountKey, int TransactionTypeNumber, string TransactionReference, SAHLPrincipal principal)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "FindLoanProcessTran");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@l_LoanNumber", AccountKey));
            prms.Add(new SqlParameter("@i_TransactionTypeNumber", TransactionTypeNumber));
            prms.Add(new SqlParameter("@s_TransactionReference", TransactionReference));
            prms.Add(new SqlParameter("@s_UserId ", principal.Identity.Name));

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        /// <summary>
        /// Get the LoanTransactions of posted CATS dsibursements for purposes of inserting DisbursementLoanTransaction linking record.
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <param name="TransactionTypeNumber"></param>
        /// <param name="TransactionInsertDate"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<IFinancialTransaction> GetCATSDisbursementLoanTransactions(int FinancialServiceKey, int TransactionTypeNumber, DateTime TransactionInsertDate, DateTime TransactionEffectiveDate, string TransactionReference, string UserID)
        {
            const string query = @"SELECT distinct FinancialTransactionKey FROM [2AM].[fin].[FinancialTransaction] ft (nolock)
                                    where FinancialServiceKey = @FinancialServiceKey
                                    and TransactionTypeKey = @TransactionTypeNumber
                                    and InsertDate = @TransactionInsertDate
                                    and EffectiveDate = @TransactionEffectiveDate
                                    and Reference = @TransactionReference
                                    and UserId = @UserId";

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", FinancialServiceKey));
            prms.Add(new SqlParameter("@TransactionTypeNumber", TransactionTypeNumber));
            prms.Add(new SqlParameter("@TransactionInsertDate", TransactionInsertDate));
            prms.Add(new SqlParameter("@TransactionEffectiveDate", TransactionEffectiveDate));
            prms.Add(new SqlParameter("@TransactionReference", TransactionReference));
            prms.Add(new SqlParameter("@UserId", UserID));

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            List<IFinancialTransaction> ltList = new List<IFinancialTransaction>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int key = Convert.ToInt32(row[0]);
                    FinancialTransaction_DAO ltDAO = FinancialTransaction_DAO.Find(key);
                    FinancialTransaction lt = new FinancialTransaction(ltDAO);
                    ltList.Add(lt);
                }
            }

            return ltList;
        }

        public IReadOnlyEventList<IFinancialTransaction> GetLoanTransactionsByTransactionTypeAndAccountKey(TransactionTypes transactionType, int accountKey)
        {
            string hql = "from FinancialTransaction_DAO lt where lt.FinancialService.Account.Key = ? and lt.TransactionType.Key = ? order by lt.InsertDate desc";

            SimpleQuery<FinancialTransaction_DAO> query = new SimpleQuery<FinancialTransaction_DAO>(hql, accountKey, (int)transactionType);

            FinancialTransaction_DAO[] lt = query.Execute();

            return new ReadOnlyEventList<IFinancialTransaction>(new DAOEventList<FinancialTransaction_DAO, IFinancialTransaction, FinancialTransaction>(new List<FinancialTransaction_DAO>(lt)));
        }

        public LoanCalculations.AmortisationScheduleDataTable GenerateAmortiseData(double LoanBalance, double Instalment, double InterestRate)
        {
            //Populate the dataset to bind to the grid
            LoanCalculations.AmortisationScheduleDataTable dt = new LoanCalculations.AmortisationScheduleDataTable();
            LoanCalculations.AmortisationScheduleRow row = null;

            double closeBalance = 20;
            int yrCnt = 0;
            int mnthCnt = 0;
            double openBalance = LoanBalance;
            double instalment = Instalment;
            double interestRate = InterestRate;
            double interestAmount = 0;
            double capitalAmount = 0;
            double instalmentDisplay = 0;

            while (closeBalance > 0)
            {
                mnthCnt += 1;
                if (row == null)
                {
                    row = dt.NewAmortisationScheduleRow();
                    row.Capital = 0D;
                    row.Interest = 0D;
                    row.Opening = openBalance;
                    row.Closing = openBalance;
                }

                interestAmount = CalculateInstalmentInterest(openBalance, interestRate);
                capitalAmount = CalculateInstalmentCapital(instalment, interestAmount);

                instalmentDisplay = instalment;
                if (openBalance > instalment)
                {
                    closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
                }
                else
                {   //removed so lasts months value does not reflect on last years figure
                    instalment = closeBalance + interestAmount;
                    capitalAmount = CalculateInstalmentCapital(instalment, interestAmount);
                    closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
                }

                row.Capital += capitalAmount;
                row.Closing = closeBalance;
                row.Payment = instalmentDisplay;
                row.Interest += interestAmount;

                openBalance = closeBalance;

                if ((mnthCnt == 12) || (closeBalance <= 0))
                {
                    mnthCnt = 0;
                    yrCnt += 1;
                    row.Period = yrCnt;

                    dt.Rows.Add(row);
                    row = dt.NewAmortisationScheduleRow();
                    row.Capital = 0D;
                    row.Interest = 0D;
                    row.Opening = openBalance;
                }
            }

            return dt;
        }

        private double CalculateCloseBalance(double OpeningBalance, double Capital)
        {
            double ClosingBalance = OpeningBalance - Capital;
            return ClosingBalance;
        }

        /// <summary>
        /// Calculate the Interest paid in a month for the Loan value at the annual interest rate
        /// </summary>
        /// <param name="LV"></param>
        /// <param name="AnnualInterestRate"></param>
        /// <returns></returns>
        private double CalculateInstalmentInterest(double LV, double AnnualInterestRate)
        {
            return LV * (AnnualInterestRate / 12);
        }

        /// <summary>
        /// Calculate the Capital portion of an instalment given the interest
        /// </summary>
        /// <param name="instalment"></param>
        /// <param name="instalmentInterest"></param>
        /// <returns></returns>
        private double CalculateInstalmentCapital(double instalment, double instalmentInterest)
        {
            return instalment - instalmentInterest;
        }
    }
}