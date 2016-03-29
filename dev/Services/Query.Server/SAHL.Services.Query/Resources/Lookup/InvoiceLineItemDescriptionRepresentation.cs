using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Lookup
{
    public class InvoiceLineItemDescriptionRepresentation : Representation, IRepresentation
    {

        private ILinkResolver linkResolver;

        public InvoiceLineItemDescriptionRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new {id = this.Id}); }
        }

    }

}
        