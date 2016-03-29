using SAHL.Services.Query.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyBankAccountListRepresentation : Representation, IListRepresentation
    {  
        
        private readonly ILinkResolver linkResolver;

        public IList<Representation> List { get; private set; }
        public IPagingRepresentation _paging { get; set; }
        public int? TotalCount { get; set; }
        public int? ListCount { get; set; }

        public ThirdPartyBankAccountListRepresentation(ILinkResolver linkResolver, IList<Representation> list) 
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
