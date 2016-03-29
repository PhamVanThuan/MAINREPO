using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Finance
{
    public class ThirdPartyInvoiceDocumentRepresentation : Representation, IRepresentation
    {
        private ILinkResolver linkResolver;

        public ThirdPartyInvoiceDocumentRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public Guid Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? AccountKey { get; set; }
        public int? StorKey { get; set; }
        public int? ThirdPartyInvoiceKey { get; set; }
        public string EmailSubject { get; set; }
        public string FromEmailAddress { get; set; }
        public string InvoiceFileName { get; set; }
        public string Category { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateProcessed { get; set; }
        public int? InvoiceStatusKey { get; set; }
        public string InvoiceStatusDescription { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? ThirdPartyKey { get; set; }

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