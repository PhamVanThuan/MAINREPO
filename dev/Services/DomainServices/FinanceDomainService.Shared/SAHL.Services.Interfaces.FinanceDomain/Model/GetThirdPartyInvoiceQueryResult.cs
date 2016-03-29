using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class GetThirdPartyInvoiceQueryResult
    {
        public int ThirdPartyInvoiceKey { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceStatusKey { get; set; }
        public int AccountKey { get; set; }
        public string SahlReference { get; set; }

        public DateTime? InvoiceDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedFromEmailAddress { get; set; }

        public Guid? ThirdPartyId { get; set; }
        public string ThirdPartyRegisteredName { get; set; }

        public decimal? AmountExcludingVAT { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? TotalAmountIncludingVAT { get; set; }

        public bool? CapitaliseInvoice { get; set; }
    }
}
