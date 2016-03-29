using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicationDebitOrderModel : ValidatableModel
    {
        public ApplicationDebitOrderModel(int applicationNumber, int debitOrderDay, int clientBankAccountKey)
        {
            this.PaymentType = FinancialServicePaymentType.DebitOrderPayment;
            this.ApplicationNumber = applicationNumber;
            this.DebitOrderDay = debitOrderDay;
            this.ClientBankAccountKey = clientBankAccountKey;
            base.Validate();
        }
        [Required]
        public FinancialServicePaymentType PaymentType { get; protected set; }

        [Required]
        [Range(1, 31, ErrorMessage = "The Debit Order Day needs to be between 1 and 31.")]
        public int DebitOrderDay { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "The ClientBankAccountKey must be provided.")]
        public int ClientBankAccountKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "The ApplicationNumber must be provided.")]
        public int ApplicationNumber { get; protected set; }
    }
}