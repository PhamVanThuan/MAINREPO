using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InvoiceLineItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public InvoiceLineItemDataModel(int thirdPartyInvoiceKey, int invoiceLineItemDescriptionKey, decimal amount, bool isVATItem, decimal? vATAmount, decimal totalAmountIncludingVAT)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InvoiceLineItemDescriptionKey = invoiceLineItemDescriptionKey;
            this.Amount = amount;
            this.IsVATItem = isVATItem;
            this.VATAmount = vATAmount;
            this.TotalAmountIncludingVAT = totalAmountIncludingVAT;
		
        }
		[JsonConstructor]
        public InvoiceLineItemDataModel(int invoiceLineItemKey, int thirdPartyInvoiceKey, int invoiceLineItemDescriptionKey, decimal amount, bool isVATItem, decimal? vATAmount, decimal totalAmountIncludingVAT)
        {
            this.InvoiceLineItemKey = invoiceLineItemKey;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InvoiceLineItemDescriptionKey = invoiceLineItemDescriptionKey;
            this.Amount = amount;
            this.IsVATItem = isVATItem;
            this.VATAmount = vATAmount;
            this.TotalAmountIncludingVAT = totalAmountIncludingVAT;
		
        }		

        public int InvoiceLineItemKey { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public int InvoiceLineItemDescriptionKey { get; set; }

        public decimal Amount { get; set; }

        public bool IsVATItem { get; set; }

        public decimal? VATAmount { get; set; }

        public decimal TotalAmountIncludingVAT { get; set; }

        public void SetKey(int key)
        {
            this.InvoiceLineItemKey =  key;
        }
    }
}