using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.CATS.Models
{
    public class PaymentBatch : ValidatableModel
    {
        [Required(ErrorMessage = "A source bank account should be provided.")]
        public BankAccountDataModel SourceAccount { get; set; }
        public decimal Amount
        {
            get { return Payments.Sum(x => x.Amount); }
        }
        [StringLength(30, ErrorMessage = "Payment batch reference length should not be longer than 30 characters.")]
        public string Reference { get; set; }

        [Required(ErrorMessage = "Payment list should be provided.")]
        public List<Payment> Payments { get; set; }
        public PaymentBatch(List<Payment> payments, BankAccountDataModel sourceAccount, decimal amount, string reference)
        {
            this.Reference = reference.Trim();
            this.Payments = payments;
            this.SourceAccount = sourceAccount;
            this.Validate();
        }
    }
}
