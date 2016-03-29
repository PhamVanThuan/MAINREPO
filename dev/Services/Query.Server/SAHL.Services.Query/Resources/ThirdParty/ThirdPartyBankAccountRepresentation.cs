using SAHL.Services.Query.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyBankAccountRepresentation : Representation, IRepresentation
    {
       private ILinkResolver linkResolver;

       public ThirdPartyBankAccountRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

       public Guid Id { get; set; }
       public int? ThirdPartyKey { get; set; }
       public int? LegalEntityKey { get; set; }
       public int? BankAccountKey { get; set; }
       public string AccountNumber { get; set; }
       public string BranchCode { get; set; }
       public string AccountName { get; set; }
       public string BankingInstitute { get; set; }

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.Id }); }
        }
    }
}
