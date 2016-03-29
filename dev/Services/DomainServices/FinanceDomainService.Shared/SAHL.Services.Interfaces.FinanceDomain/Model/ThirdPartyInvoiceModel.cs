using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class ThirdPartyInvoiceModel : ValidatableModel
    {
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "A ThirdPartyInvoiceKey must be provided.")]
        public int ThirdPartyInvoiceKey { get; protected set; }

        [Required(ErrorMessage = "A ThirdPartyId must be provided.")]
        public Guid ThirdPartyId { get; protected set; }

        [Required(ErrorMessage = "An InvoiceNumber must be provided.")]
        [DataType(DataType.Text)]
        public string InvoiceNumber { get; protected set; }

        public DateTime? InvoiceDate { get; protected set; }

        public IEnumerable<InvoiceLineItemModel> LineItems { get; protected set; }

        public decimal AmountExcludingVAT { get; private set; }

        public decimal VATAmount { get; private set; }

        public decimal TotalAmountIncludingVAT { get; private set; }

        public string ApproverCurrentUserCapabilities { get; set; }

        public bool CapitaliseInvoice { get; protected set; }

        [MaxLength(30, ErrorMessage = "The payment reference cannot be longer than 30 characters.")]
        public string PaymentReference { get; protected set; }

        public ThirdPartyInvoiceModel(int thirdPartyInvoiceKey, Guid thirdPartyId, string invoiceNumber, DateTime? invoiceDate,
            IEnumerable<InvoiceLineItemModel> lineItems, bool capitaliseInvoice, string paymentReference)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.ThirdPartyId = thirdPartyId;
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceDate = invoiceDate;
            this.LineItems = lineItems;
            this.CapitaliseInvoice = capitaliseInvoice;
            this.AmountExcludingVAT = lineItems.Sum(li => li.AmountExcludingVAT);
            this.VATAmount = lineItems.Sum(li => li.VATAmount);
            this.TotalAmountIncludingVAT = lineItems.Sum(li => li.TotalAmountIncludingVAT);
            this.PaymentReference = paymentReference;
            this.Validate();
        }
    }
}