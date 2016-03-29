using SAHL.Core.Data.Models._2AM;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.Validation;

namespace SAHL.Services.Interfaces.CATS.Models
{
    public class Payment : ValidatableModel
    {
        [Required(ErrorMessage = "Payment Target bank account should not be provided.")]
        public BankAccountDataModel TargetAccount { get; protected set; }

        [Range(1, 999999999999999, ErrorMessage = "The Amount must be between 1 and 999999999999999.")]
        public decimal Amount { get; protected set; }

        [StringLength(30, ErrorMessage = "Payment reference length should not be longer than 30 characters.")]
        public string Reference { get; protected set; }

        public Payment(BankAccountDataModel targetAccount, decimal amount, string reference)
        {
            this.Amount = amount;
            this.Reference = reference.Trim();
            this.TargetAccount = targetAccount;
            this.Validate();
        }
    }

}
