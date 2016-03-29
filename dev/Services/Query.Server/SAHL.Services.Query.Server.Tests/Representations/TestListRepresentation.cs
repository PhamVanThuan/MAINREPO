using System.Collections;
using System.Collections.Generic;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Resources;
using WebApi.Hal;
using WebApi.Hal.Interfaces;

namespace SAHL.Services.Query.Server.Tests.Representations
{
    public class TestListRepresentation : Representation, IListRepresentation
    {
        public ILinkResolver LinkResolver { get; set; }
        public IList<Representation> List { get; set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        public TestListRepresentation() { }

        public TestListRepresentation(ILinkResolver linkResolver, IList<Representation> list) 
        {
            LinkResolver = linkResolver;
            List = list;
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