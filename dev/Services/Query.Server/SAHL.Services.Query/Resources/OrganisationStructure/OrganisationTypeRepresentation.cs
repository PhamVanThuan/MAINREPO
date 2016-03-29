using System;
using System.Collections.Generic;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.OrganisationStructure
{
    public class OrganisationTypeRepresentation : Representation
    {
        private readonly ILinkResolver linkResolver;

        public int Id { get; set; }
        public string Name { get; set; }

        public OrganisationTypeRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [Obsolete("Use ValueInjecter to set properties")]
        internal OrganisationTypeRepresentation(ILinkResolver linkResolver, int id, string name)
        {
            this.linkResolver = linkResolver;
            this.Id = id;
            this.Name = name;
        }

        public override string Rel
        {
            get { return linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get  { return linkResolver.GetHref(this, new { id = this.Id }); }
        }
    }
}