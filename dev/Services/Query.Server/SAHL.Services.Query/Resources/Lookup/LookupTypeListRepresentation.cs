using System.Collections.Generic;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.LinkTemplates;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Lookup
{
    public class LookupTypeListRepresentation : Representation, IListRepresentation
    {

        public IList<LookupTypeRepresentation> Lookups { get; set; }

        public LookupTypeListRepresentation(IList<LookupTypeRepresentation> lookups)
        {
            Lookups = lookups;
        }

        private Link link;
        private Link Link
        {
            get
            {
                if (this.link == null)
                {
                    this.link = Representations.LinkMetadata[GetType()].ToHalLink();
                }
                return this.link;
            }
        }

        public override string Rel
        {
            get { return this.Link.Rel; }
        }

        public override string Href
        {
            get { return this.Link.Href; }
        }

        public IList<Representation> List { get; private set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }
    }
}