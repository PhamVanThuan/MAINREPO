using System.Collections.Generic;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.OrganisationStructure
{
    public class OrganisationTypeListRepresentation : Representation, IListRepresentation
    {
        private readonly ILinkResolver linkResolver;
        public IEnumerable<OrganisationTypeRepresentation> Types { get; private set; }

        public OrganisationTypeListRepresentation(ILinkResolver linkResolver, IEnumerable<OrganisationTypeRepresentation> types)
        {
            this.linkResolver = linkResolver;
            this.Types = types;
        }

        public override string Rel 
        {
            get { return linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return linkResolver.GetHref(this); }
        }

        public IList<Representation> List { get; private set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }
    }
}