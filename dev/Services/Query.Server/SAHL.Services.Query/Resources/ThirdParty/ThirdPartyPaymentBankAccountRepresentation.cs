using SAHL.Services.Query.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyPaymentBankAccountRepresentation : Representation, IRepresentation
    {
         
        private ILinkResolver linkResolver;

        public ThirdPartyPaymentBankAccountRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }
        public int Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public Guid? ThirdPartyId { get; set; }
        public int? BankAccountKey { get; set; }
        public string AccountName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string BeneficiaryBankCode { get; set; } 

        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get { return this.linkResolver.GetHref(this, new { id = this.ThirdPartyId }); }
        }
        
        
    }
}