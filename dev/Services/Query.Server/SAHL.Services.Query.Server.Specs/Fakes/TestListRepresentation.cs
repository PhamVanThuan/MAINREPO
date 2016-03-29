using System.Collections.Generic;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Resources;

namespace SAHL.Services.Query.Server.Specs.Fakes
{
    public class TestListRepresentation : WebApi.Hal.Representation, IListRepresentation
    {
        public ILinkResolver LinkResolver { get; set; }
        public IList<WebApi.Hal.Representation> List { get; set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        public TestListRepresentation() { }

        public TestListRepresentation(ILinkResolver linkResolver, IList<WebApi.Hal.Representation> list) 
        {
            this.LinkResolver = linkResolver;
            this.List = list;
        }

        public override string Rel
        {
            get { return this.LinkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.LinkResolver.GetHref(this); }
        }

    }

}