using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.CATS.Models
{
    public class CATSPaymentBatchItemModel : ValidatableModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "The Legal Entity must be provided")]
        public int LegalEntityKey { get; protected set; }

        [Required]
        public int GenericKey { get; protected set; }

        [Required]
        public int GenericTypeKey { get; protected set; }

        [Required]
        public int AccountKey { get; protected set; }

        [Range(1, 999999999999999, ErrorMessage = "The Amount must be between 1 and 999999999999999.")]
        public decimal Amount { get; protected set; }

        [Required]
        public int CATSPaymentBatchKey { get; protected set; }

        [Required]
        public int SourceBankAccountKey { get; protected set; }

        [Required]
        public int TargetBankAccountKey { get; protected set; }

        public string SahlReferenceNumber { get; protected set; }

        public string SourceReferenceNumber { get; protected set; }

        [Required]
        public string TargetName { get; protected set; }

        public string ExternalReference { get; protected set; }

        public string EmailAddress { get; protected set; }

        public bool Processed { get; protected set; }

        public CATSPaymentBatchItemModel(int legalEntityKey, int genericKey, int genericTypeKey, int accountKey, decimal amount, int catsPaymentBatchKey, int sourceBankAccountKey,
            int targetBankAccountKey, string sahlReferenceNumber, string sourceReferenceNumber, string targetName, string externalReference, string emailAddress, bool processed)
        {
            LegalEntityKey = legalEntityKey;
            GenericKey = genericKey;
            GenericTypeKey = genericTypeKey;
            AccountKey = accountKey;
            Amount = amount;
            CATSPaymentBatchKey = catsPaymentBatchKey;
            SourceBankAccountKey = sourceBankAccountKey;
            TargetBankAccountKey = targetBankAccountKey;
            SahlReferenceNumber = sahlReferenceNumber;
            SourceReferenceNumber = sourceReferenceNumber;
            TargetName = targetName;
            ExternalReference = externalReference;
            EmailAddress = emailAddress;
            Processed = processed;

            this.Validate();
        }
    }
}
