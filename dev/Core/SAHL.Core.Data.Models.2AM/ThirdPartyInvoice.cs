using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ThirdPartyInvoiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ThirdPartyInvoiceDataModel(string sahlReference, int invoiceStatusKey, int accountKey, Guid? thirdPartyId, string invoiceNumber, DateTime? invoiceDate, string receivedFromEmailAddress, decimal? amountExcludingVAT, decimal? vATAmount, decimal? totalAmountIncludingVAT, bool? capitaliseInvoice, DateTime? receivedDate, string paymentReference)
        {
            this.SahlReference = sahlReference;
            this.InvoiceStatusKey = invoiceStatusKey;
            this.AccountKey = accountKey;
            this.ThirdPartyId = thirdPartyId;
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceDate = invoiceDate;
            this.ReceivedFromEmailAddress = receivedFromEmailAddress;
            this.AmountExcludingVAT = amountExcludingVAT;
            this.VATAmount = vATAmount;
            this.TotalAmountIncludingVAT = totalAmountIncludingVAT;
            this.CapitaliseInvoice = capitaliseInvoice;
            this.ReceivedDate = receivedDate;
            this.PaymentReference = paymentReference;
		
        }
		[JsonConstructor]
        public ThirdPartyInvoiceDataModel(int thirdPartyInvoiceKey, string sahlReference, int invoiceStatusKey, int accountKey, Guid? thirdPartyId, string invoiceNumber, DateTime? invoiceDate, string receivedFromEmailAddress, decimal? amountExcludingVAT, decimal? vATAmount, decimal? totalAmountIncludingVAT, bool? capitaliseInvoice, DateTime? receivedDate, string paymentReference)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.SahlReference = sahlReference;
            this.InvoiceStatusKey = invoiceStatusKey;
            this.AccountKey = accountKey;
            this.ThirdPartyId = thirdPartyId;
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceDate = invoiceDate;
            this.ReceivedFromEmailAddress = receivedFromEmailAddress;
            this.AmountExcludingVAT = amountExcludingVAT;
            this.VATAmount = vATAmount;
            this.TotalAmountIncludingVAT = totalAmountIncludingVAT;
            this.CapitaliseInvoice = capitaliseInvoice;
            this.ReceivedDate = receivedDate;
            this.PaymentReference = paymentReference;
		
        }		

        public int ThirdPartyInvoiceKey { get; set; }

        public string SahlReference { get; set; }

        public int InvoiceStatusKey { get; set; }

        public int AccountKey { get; set; }

        public Guid? ThirdPartyId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string ReceivedFromEmailAddress { get; set; }

        public decimal? AmountExcludingVAT { get; set; }

        public decimal? VATAmount { get; set; }

        public decimal? TotalAmountIncludingVAT { get; set; }

        public bool? CapitaliseInvoice { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public string PaymentReference { get; set; }

        public void SetKey(int key)
        {
            this.ThirdPartyInvoiceKey =  key;
        }
    }
}