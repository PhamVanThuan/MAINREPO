using System;
using System.Collections.Generic;
using SAHL.Services.Query.LinkTemplates;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Lookup
{
    public class LookupListRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;
        private string LookupType { get; set; }

        public IList<LookupRepresentation> Lookups { get; set; }

        public LookupListRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [Obsolete("Use ValueInjecter to set properties")]
        internal LookupListRepresentation(ILinkResolver linkResolver, IList<LookupRepresentation> lookups, string lookupType)
        {
            this.linkResolver = linkResolver;
            Lookups = lookups;
            LookupType = lookupType;
        }

        protected override void CreateHypermedia()
        {
            Links.Add(new Link{ Href = LookupLinkTemplate.GetLookups.Href, Rel = "parent" });
        }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this); }
        }

    }

}