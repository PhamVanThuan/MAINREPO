using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CATSPaymentBatchItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CATSPaymentBatchItemDataModel(int genericKey, int genericTypeKey, int accountKey, decimal amount, int sourceBankAccountKey, int targetBankAccountKey, int cATSPaymentBatchKey, string sahlReferenceNumber, string sourceReferenceNumber, string targetName, string externalReference, string emailAddress, int legalEntityKey, bool? processed)
        {
            this.GenericKey = genericKey;
            this.GenericTypeKey = genericTypeKey;
            this.AccountKey = accountKey;
            this.Amount = amount;
            this.SourceBankAccountKey = sourceBankAccountKey;
            this.TargetBankAccountKey = targetBankAccountKey;
            this.CATSPaymentBatchKey = cATSPaymentBatchKey;
            this.SahlReferenceNumber = sahlReferenceNumber;
            this.SourceReferenceNumber = sourceReferenceNumber;
            this.TargetName = targetName;
            this.ExternalReference = externalReference;
            this.EmailAddress = emailAddress;
            this.LegalEntityKey = legalEntityKey;
            this.Processed = processed;
		
        }
		[JsonConstructor]
        public CATSPaymentBatchItemDataModel(int cATSPaymentBatchItemKey, int genericKey, int genericTypeKey, int accountKey, decimal amount, int sourceBankAccountKey, int targetBankAccountKey, int cATSPaymentBatchKey, string sahlReferenceNumber, string sourceReferenceNumber, string targetName, string externalReference, string emailAddress, int legalEntityKey, bool? processed)
        {
            this.CATSPaymentBatchItemKey = cATSPaymentBatchItemKey;
            this.GenericKey = genericKey;
            this.GenericTypeKey = genericTypeKey;
            this.AccountKey = accountKey;
            this.Amount = amount;
            this.SourceBankAccountKey = sourceBankAccountKey;
            this.TargetBankAccountKey = targetBankAccountKey;
            this.CATSPaymentBatchKey = cATSPaymentBatchKey;
            this.SahlReferenceNumber = sahlReferenceNumber;
            this.SourceReferenceNumber = sourceReferenceNumber;
            this.TargetName = targetName;
            this.ExternalReference = externalReference;
            this.EmailAddress = emailAddress;
            this.LegalEntityKey = legalEntityKey;
            this.Processed = processed;
		
        }		

        public int CATSPaymentBatchItemKey { get; set; }

        public int GenericKey { get; set; }

        public int GenericTypeKey { get; set; }

        public int AccountKey { get; set; }

        public decimal Amount { get; set; }

        public int SourceBankAccountKey { get; set; }

        public int TargetBankAccountKey { get; set; }

        public int CATSPaymentBatchKey { get; set; }

        public string SahlReferenceNumber { get; set; }

        public string SourceReferenceNumber { get; set; }

        public string TargetName { get; set; }

        public string ExternalReference { get; set; }

        public string EmailAddress { get; set; }

        public int LegalEntityKey { get; set; }

        public bool? Processed { get; set; }

        public void SetKey(int key)
        {
            this.CATSPaymentBatchItemKey =  key;
        }
    }
}