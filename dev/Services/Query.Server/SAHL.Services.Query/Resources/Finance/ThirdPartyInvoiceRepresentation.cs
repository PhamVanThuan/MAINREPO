using SAHL.Services.Query.Core;
using System;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Finance
{
    public class ThirdPartyInvoiceRepresentation : Representation, IRepresentation
    {

        private ILinkResolver linkResolver;

        public ThirdPartyInvoiceRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? LegalEntityKey { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? AccountKey { get; set; }
        public int? InvoiceStatusKey { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string SahlReference { get; set; }
        public string ReceivedFromEmailAddress { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public bool? CapitaliseInvoice { get; set; }
        public decimal? AmountExcludingVAT { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? TotalAmountIncludingVAT { get; set; }
        public string ClientName { get; set; }
        public string PaymentReference { get; set; }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.Id }); }
        }
    }
}
