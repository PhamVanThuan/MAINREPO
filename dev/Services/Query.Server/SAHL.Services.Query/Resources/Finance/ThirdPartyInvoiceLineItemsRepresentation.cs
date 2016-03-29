using SAHL.Services.Query.Core;
using System;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Finance
{
    public class ThirdPartyInvoiceLineItemsRepresentation : Representation, IRepresentation
    {

        private ILinkResolver linkResolver;

        public ThirdPartyInvoiceLineItemsRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public int? ThirdPartyInvoiceKey { get; set; }
        public int? ThirdPartyId { get; set; }
        public string InvoiceStatus { get; set; }
        public string LineItemType { get; set; }
        public string LineItemDesc { get; set; }
        public int? InvoiceLineItemDescriptionKey { get; set; }
        public int? InvoiceLineItemCategoryKey { get; set; }
        public decimal? LineItemAmount { get; set; }
        public Boolean? IsVatable { get; set; }
        public Boolean? CapitaliseInvoice { get; set; }
        public decimal? LineItemVatAmount { get; set; }
        public decimal? LineItemTotalAmtInclVAT { get; set; }

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

