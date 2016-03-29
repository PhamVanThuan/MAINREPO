using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Shared.BusinessModel.Models
{
    public class ReversalTransactionModel : ValidatableModel
    {
        [Required(ErrorMessage = "FinancialServiceKey is required")]
        [Range(1, Int64.MaxValue, ErrorMessage = "FinancialServiceKey must be greater than 0")]
        public int FinancialTransactionKey { get; protected set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; protected set; }

        public ReversalTransactionModel(int financialTransactionKey, string userId)
        {
            FinancialTransactionKey = financialTransactionKey;
            UserId = userId;

            Validate();
        }
    }
}
