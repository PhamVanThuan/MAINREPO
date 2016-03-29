using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.DataSets;

//using System.Collections.Generic;
//using System.Text;
//using SAHL.Common.Collections.Interfaces;
//using System.Data;
//using SAHL.Common.DataAccess;
//using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ILoanTransactionRepository
    {
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
        List<IFinancialTransaction> GetCATSDisbursementLoanTransactions(int FinancialServiceKey, int TransactionTypeNumber, DateTime TransactionInsertDate, DateTime TransactionEffectiveDate, string TransactionReference, string UserID);

        IFinancialTransaction GetLoanTransactionByLoanTransactionNumber(int LoanTransactionNumber);

        void PostTransactionByAccountKey(int AccountKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, double TransactionAmount, string TransactionReference, string ADUserName);

        void PostTransactionByFinancialServiceKey(int FinancialServiceKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, double TransactionAmount, string TransactionReference, string ADUserName);

        DataTable GetTransactions(string StatementName, string ApplicationName, List<SqlParameter> Parameters, int FetchRows);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionNumber"></param>
        /// <param name="UserID"></param>
        /// <param name="IsArrear"></param>
        void RollbackTransaction(int AccountKey, int TransactionNumber, string UserID, bool IsArrear);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetAdvancesDisbursedThisYearByAccountKey(int key);

        /// <summary>
        /// Return the most recent transaction for the financial service by type
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="financialServiceKey"></param>
        /// <param name="includeRolledbackTransactions"></param>
        /// <returns></returns>
        IFinancialTransaction GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(TransactionTypes transactionType, int financialServiceKey, bool includeRolledbackTransactions);

        /// <summary>
        /// Return the most recent transaction for the Account by type
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="accountKey"></param>
        /// <param name="includeRolledbackTransactions"></param>
        /// <returns></returns>
        IFinancialTransaction GetLastLoanTransactionByTransactionTypeAndAccountKey(TransactionTypes transactionType, int accountKey, bool includeRolledbackTransactions);

        IReadOnlyEventList<IFinancialTransaction> GetLoanTransactionsByTransactionTypeAndAccountKey(TransactionTypes transactionType, int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialServiceKey"></param>
        /// <param name="transactionTypeKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="amount"></param>
        /// <param name="reference"></param>
        /// <param name="userID"></param>
        /// <returns>Messages in the Domain Message Collection for any failure.</returns>
        void pProcessTran(int financialServiceKey, int transactionTypeKey, DateTime effectiveDate, double amount, string reference, string userID);

        void pLoanProcessRollbackTran(int LoanTransactionNumber, SAHLPrincipal principal);

        int FindLoanProcessTran(int AccountKey, int TransactionTypeKey, DateTime TransactionEffectiveDate, string TransactionReference, string UserID);

        DataTable FindLoanProcessTran(int AccountKey, int TransactionTypeNumber, string TransactionReference, SAHLPrincipal principal);

        void ProcessAccountPaymentTran(int accountKey, int transactionTypeKey, double amount, string reference, string ADUserName, DateTime effectivedate);

        LoanCalculations.AmortisationScheduleDataTable GenerateAmortiseData(double LoanBalance, double Instalment, double InterestRate);
    }
}