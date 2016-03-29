using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// Interface for BatchTransactions Repository
    /// </summary>
    public interface IBulkBatchRepository
    {
        /// <summary>
        /// Get Data Table of Batch Loan Transactions
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionTypeNumber"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IReadOnlyEventList<IFinancialTransaction> GetBatchLoanTransactions(int AccountKey, int TransactionTypeNumber, DateTime TransactionEffectiveDate, string TransactionReference, string ADUserName);

        //DataTable GetBatchLoanTransactions(int AccountKey, int TransactionTypeNumber, DateTime TransactionEffectiveDate, string TransactionReference, string ADUserName);
        /// <summary>
        /// Get Batch Transaction By BatchTransaction Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IBatchTransaction GetBatchTransactionByKey(int Key);

        /// <summary>
        /// Post batch Transaction
        /// </summary>
        /// <param name="BatchTransactionKey"></param>
        /// <param name="AccountKey"></param>
        /// <param name="TransactionTypeNumber"></param>
        /// <param name="TransactionEffectiveDate"></param>
        /// <param name="TransactionAmount"></param>
        /// <param name="TransactionReference"></param>
        /// <param name="ADUserName"></param>
        void PostTransaction(int BatchTransactionKey, int AccountKey, int TransactionTypeNumber, DateTime TransactionEffectiveDate, float TransactionAmount, string TransactionReference, string ADUserName);

        /// <summary>
        /// Get Empty BulkBatch
        /// </summary>
        /// <returns></returns>
        IBulkBatch GetEmptyBulkBatch();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IBulkBatchParameter GetEmptyBulkBatchParameter();

        /// <summary>
        /// Save BulkBatch
        /// </summary>
        /// <param name="bk"></param>
        void SaveBulkBatch(IBulkBatch bk);

        /// <summary>
        /// Get tables of Bulk Batch transactions for Update
        /// </summary>
        /// <param name="BulkBatchKey"></param>
        /// <returns></returns>
        DataTable GetUpdateBulkTransactionBatchByBulkBatchKey(int BulkBatchKey);

        /// <summary>
        /// Get table of BulkBatchTransactions for dd
        /// </summary>
        /// <param name="SubsidyProviderKey"></param>
        /// <param name="OriginationSourceKeys"></param>
        /// <returns></returns>
        DataTable GetAddBulkTransactionBatchBySubsidyProviderKey(int SubsidyProviderKey, int[] OriginationSourceKeys);

        /// <summary>
        /// Get Batch Transactions by BulkBatchKey
        /// </summary>
        /// <param name="BulkBatchKey"></param>
        /// <returns></returns>
        DataTable GetBatchTransactionByBulkBatchKey(int BulkBatchKey);

        /// <summary>
        /// Get Empty Batch Transaction
        /// </summary>
        /// <returns></returns>
        IBatchTransaction GetEmptyBatchTransaction();

        /// <summary>
        /// Save batch Transaction
        /// </summary>
        /// <param name="bt"></param>
        void SaveBatchTransaction(IBatchTransaction bt);

        /// <summary>
        /// Get BulkBatchTransaction by BulkBatchKey
        /// </summary>
        /// <param name="bulkBatchTypeKey"></param>
        /// <returns></returns>
        IList<IBulkBatch> GetBulkBatchTransactionsByBulkBatchTypeKey(int bulkBatchTypeKey);

        /// <summary>
        /// Get BatchLog by BatchKey and Message Type
        /// </summary>
        /// <param name="bulkBatchKey"></param>
        /// <param name="messageTypeKey"></param>
        /// <returns></returns>
        IList<IBulkBatchLog> GetBatchLogByBatchKeyMessageType(int bulkBatchKey, int messageTypeKey);

        /// <summary>
        /// Get BulkBatch by Type and Status
        /// </summary>
        /// <param name="bulkBatchTypeKey"></param>
        /// <param name="bulkBatchStatusKey"></param>
        /// <returns></returns>
        IList<IBulkBatch> GetBulkBatchByBulkBatchTypeAndStatusKey(int bulkBatchTypeKey, int bulkBatchStatusKey);

        /// <summary>
        /// Get BatchTransactions By BulkBatchKey
        /// </summary>
        /// <param name="bulkBatchKey"></param>
        /// <returns></returns>
        IList<IBatchTransaction> GetBatchTransactionsByBulkBatchKey(int bulkBatchKey);

        //IList<IBatchTransaction> GetBatchTransactionByBulkBatchKey(int bulkBatchKey, int batchTransactionStatusKey);
        /// <summary>
        /// Get SubsidyProvider By SubsidyProviderTypeKey
        /// </summary>
        /// <param name="subsidyProviderTypeKey"></param>
        /// <returns></returns>
        IList<ISubsidyProvider> GetSubsidyProviderBySubsidyProviderTypeKey(int subsidyProviderTypeKey);

        /// <summary>
        /// Get TransactionType by Key
        /// </summary>
        /// <param name="transactionTypeKey"></param>
        /// <returns></returns>
        ITransactionType GetTransactionTypeByKey(int transactionTypeKey);

        /// <summary>
        /// Get FinancialTransaction Types
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        IReadOnlyEventList<ITransactionType> GetLoanTransactionTypes(SAHLPrincipal principal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="ltTypeKeys">A single or comma separated list of LoanTransactionType Keys</param>
        /// <returns></returns>
        IReadOnlyEventList<ITransactionType> GetLoanTransactionTypesByKeys(SAHLPrincipal principal, string ltTypeKeys);

        /// <summary>
        /// Get Arrear FinancialTransaction Types
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        IReadOnlyEventList<ITransactionType> GetLoanTransactionTypesArrears(SAHLPrincipal principal);

        /// <summary>
        /// Get BulkBatch by key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IBulkBatch GetBulkBatchByKey(int Key);

        /// <summary>
        /// Delete Batch and Transactions
        /// </summary>
        /// <param name="bb"></param>
        void DeleteBulkBatch(IBulkBatch bb);

        /// <summary>
        /// Delete Batch and Transactions
        /// </summary>
        /// <param name="BulkBatchKey"></param>
        void DeleteBulkBatch(int BulkBatchKey);
    }
}