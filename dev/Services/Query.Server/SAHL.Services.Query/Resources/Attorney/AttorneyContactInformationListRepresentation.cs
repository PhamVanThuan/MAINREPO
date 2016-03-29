using System.Collections.Generic;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Metadata;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    [ServiceGenerationToolExclude]
    public class AttorneyContactInformationListRepresentation : Representation, IListRepresentation
    {

        private ILinkResolver linkResolver { get; set; }
        public IList<Representation> List { get; private set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        public AttorneyContactInformationListRepresentation(ILinkResolver linkResolver, IList<Representation> list) 
        {
            this.linkResolver = linkResolver;
            List = list;
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