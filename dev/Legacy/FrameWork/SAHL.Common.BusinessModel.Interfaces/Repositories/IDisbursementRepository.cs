using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IDisbursementRepository
    {
        /// <summary>
        /// Set the Margin and/or BaseRate on all MortgageLoans for the Account.
        /// Category will be assessed and updated if required.
        /// This method saves objects internally, so no need to save again on completion.
        /// NB! This method must be called within a Transaction.
        /// </summary>
        /// <param name="account">IAccount to update</param>
        /// <param name="margin">IMargin to save against the Account</param>
        /// <param name="userid">User making the change</param>
        /// <param name="baseRateReset">For CAP, updating the Base Rate</param>
        void UpdateRate(IAccount account, IMargin margin, string userid, bool baseRateReset);

        IReadOnlyEventList<IDisbursementTransactionType> GetDisbursementTransactionTypes(SAHLPrincipal principal);

        IReadOnlyEventList<IDisbursement> GetDisbursmentsByParentAccountKeyAndStatus(int ParentAccountKey, int DisbursementStatusKey);

        DataTable GetDisbursementLoanTransactions(int AccountKey, SAHLPrincipal principal);

        DataTable GetDisbursementRollbackTransactions(int AccountKey, int[] LoanTransactionNumbers);

        IDisbursement CreateEmptyDisbursement();

        void SaveDisbursement(IReadOnlyEventList<IDisbursement> disbursementList, double totalAmount);

        IDisbursement GetDisbursementByKey(int Key);

        void DeleteDisbursement(IDisbursement IObj);

        IReadOnlyEventList<IDisbursement> GetDisbursementByLoanTransactionNumber(decimal LoanTransactionNumber);

        /// <summary>
        /// Added as part of revamp. New API for posting the fin tran and linking the disbursement ref: #20307
        /// </summary>
        /// <param name="disbursementKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="reference"></param>
        /// <param name="userID"></param>
        void PostDisbursementTransaction(int disbursementKey, DateTime effectiveDate, string reference, string userID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        void ReturnDisbursedLoanToRegistration(int accountKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationKey"></param>
        void DisburseFundsForUnsecuredLendingApplication(int applicationKey);
    }
}