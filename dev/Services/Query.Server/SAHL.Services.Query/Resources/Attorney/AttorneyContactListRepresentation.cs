using System.Collections.Generic;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    public class AttorneyContactListRepresentation : Representation, IListRepresentation
    {
        private ILinkResolver LinkResolver { get; set; }
        public IList<Representation> List { get; set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        public AttorneyContactListRepresentation(ILinkResolver linkResolver, IList<Representation> list) 
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