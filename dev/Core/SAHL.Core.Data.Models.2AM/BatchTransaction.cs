using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchTransactionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BatchTransactionDataModel(int bulkBatchKey, int accountKey, int? legalEntityKey, int transactionTypeNumber, DateTime effectiveDate, double amount, string reference, string userID, int batchTransactionStatusKey)
        {
            this.BulkBatchKey = bulkBatchKey;
            this.AccountKey = accountKey;
            this.LegalEntityKey = legalEntityKey;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.EffectiveDate = effectiveDate;
            this.Amount = amount;
            this.Reference = reference;
            this.UserID = userID;
            this.BatchTransactionStatusKey = batchTransactionStatusKey;
		
        }
		[JsonConstructor]
        public BatchTransactionDataModel(int batchTransactionKey, int bulkBatchKey, int accountKey, int? legalEntityKey, int transactionTypeNumber, DateTime effectiveDate, double amount, string reference, string userID, int batchTransactionStatusKey)
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
		
        }		

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

        public void SetKey(int key)
        {
            this.BatchTransactionKey =  key;
        }
    }
}