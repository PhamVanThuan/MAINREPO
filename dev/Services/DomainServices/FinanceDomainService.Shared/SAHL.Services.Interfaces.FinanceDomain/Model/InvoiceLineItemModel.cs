using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class InvoiceLineItemModel : ValidatableModel
    {
        public int? InvoiceLineItemKey { get; protected set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "A ThirdPartyInvoiceKey must be provided.")]
        public int ThirdPartyInvoiceKey { get; protected set; }


        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "A description for the line item must be provided.")]
        public int InvoiceLineItemDescriptionKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "The line item amount should be greater than R0.00.")]
        public decimal AmountExcludingVAT { get; protected set; }

        public bool IsVATItem { get; protected set; }

        public decimal VATAmount { get; private set; }

        public decimal TotalAmountIncludingVAT { get; private set; }

        public InvoiceLineItemModel(int? invoiceLineItemKey, int thirdPartyInvoiceKey, int invoiceLineItemDescriptionKey, decimal amountExcludingVAT, bool isVATItem)
        {
            this.InvoiceLineItemKey = invoiceLineItemKey;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InvoiceLineItemDescriptionKey = invoiceLineItemDescriptionKey;
            this.AmountExcludingVAT = amountExcludingVAT;
            this.IsVATItem = isVATItem;
            if (isVATItem)
            {
                this.VATAmount = 0;
                this.VATAmount = (amountExcludingVAT * 14) / 100;
            }
            this.TotalAmountIncludingVAT = amountExcludingVAT + this.VATAmount;
            this.Validate();
        }
    }
}
