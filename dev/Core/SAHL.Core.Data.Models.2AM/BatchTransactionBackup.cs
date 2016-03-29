using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchTransactionBackupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BatchTransactionBackupDataModel(int batchTransactionKey, int bulkBatchKey, int accountKey, int? legalEntityKey, int transactionTypeNumber, DateTime effectiveDate, double amount, string reference, string userID, int batchTransactionStatusKey, string historyType)
        {
            this.BatchTransactionKey = batchTransactionKey;
            this.BulkBatchKey = bulkBatchKey;
            this.AccountKey = accountKey;
            this.LegalEntityKey = legalEntityKey;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.EffectiveDate = effectiveDate;
            this.Amount = amount;
            this.Reference = reference;
            this.UserID = userID;
            this.BatchTransactionStatusKey = batchTransactionStatusKey;
            this.HistoryType = historyType;
		
        }
		[JsonConstructor]
        public BatchTransactionBackupDataModel(int batchTransactionBackUpKey, int batchTransactionKey, int bulkBatchKey, int accountKey, int? legalEntityKey, int transactionTypeNumber, DateTime effectiveDate, double amount, string reference, string userID, int batchTransactionStatusKey, string historyType)
        {
            this.BatchTransactionBackUpKey = batchTransactionBackUpKey;
            this.BatchTransactionKey = batchTransactionKey;
            this.BulkBatchKey = bulkBatchKey;
            this.AccountKey = accountKey;
            this.LegalEntityKey = legalEntityKey;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.EffectiveDate = effectiveDate;
            this.Amount = amount;
            this.Reference = reference;
            this.UserID = userID;
            this.BatchTransactionStatusKey = batchTransactionStatusKey;
            this.HistoryType = historyType;
		
        }		

        public int BatchTransactionBackUpKey { get; set; }

        public int BatchTransactionKey { get; set; }

        public int BulkBatchKey { get; set; }

        public int AccountKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int TransactionTypeNumber { get; set; }

        public DateTime EffectiveDate { get; set; }

        public double Amount { get; set; }

        public string Reference { get; set; }

        public string UserID { get; set; }

        public int BatchTransactionStatusKey { get; set; }

        public string HistoryType { get; set; }

        public void SetKey(int key)
        {
            this.BatchTransactionBackUpKey =  key;
        }
    }
}