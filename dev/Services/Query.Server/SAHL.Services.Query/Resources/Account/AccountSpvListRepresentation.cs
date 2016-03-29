using System.Collections.Generic;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Account
{
    [ServiceGenerationToolExclude]
    public class AccountSpvListRepresentation : Representation, IListRepresentation
    {

        private ILinkResolver linkResolver { get; set; }
        public IList<Representation> List { get; private set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }


        public AccountSpvListRepresentation(ILinkResolver linkResolver, IList<Representation> list)
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