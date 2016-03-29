using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyRepresentation : Representation, IRepresentation
    {
         
        private ILinkResolver linkResolver;

        public ThirdPartyRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public Guid Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string GeneralStatusDescription { get; set; }
        public int? LegalEntityTypeKey { get; set; }
        public string LegaLEntityStatus { get; set; }
        public int? ThirdPartyTypeKey { get; set; }
        public string ThirdPartyTypeDescription { get; set; }

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