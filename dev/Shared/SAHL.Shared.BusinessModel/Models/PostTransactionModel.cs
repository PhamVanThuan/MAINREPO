using System;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.Validation;

namespace SAHL.Shared.BusinessModel.Models
{
    public class PostTransactionModel : ValidatableModel
    {
        public PostTransactionModel(int financialServiceKey, int transactionTypeKey, decimal amount, DateTime effectiveDate, string reference, string userId)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.Amount = amount;
            this.EffectiveDate = effectiveDate;
            this.Reference = reference;
            this.UserId = userId;
            Validate();
        }

        [Required(ErrorMessage = "FinancialServiceKey is required")]
        [Range(1, Int64.MaxValue, ErrorMessage = "FinancialServiceKey must be greater than 0")]
        public int FinancialServiceKey { get; protected set; }

        [Required(ErrorMessage = "TransactionTypeKey is required")]
        [Range(1, Int32.MaxValue, ErrorMessage = "TransactionTypeKey must be greater than 0")]
        public int TransactionTypeKey { get; protected set; }

        [Required(ErrorMessage = "Transaction Amount is required")]
        [Range(0.01, Int32.MaxValue, ErrorMessage = "Transaction Amount must be greater than R0.00")]
        public decimal Amount { get; protected set; }

        [Required(ErrorMessage = "Effective Date is required")]
        public DateTime EffectiveDate { get; protected set; }

        [Required(ErrorMessage = "Reference is required")]
        public string Reference { get; protected set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; protected set; }
    }
}