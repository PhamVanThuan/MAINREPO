using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyAttorneyRepresentation : Representation, IRepresentation
    {
         
        private ILinkResolver linkResolver;

        public ThirdPartyAttorneyRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public Guid Id { get; set; }
        public int? ThirdPartyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }

        public bool? IsLitigationAttorney { get; set; }
        public bool? IsRegistrationAttorney { get; set; }
        public bool? IsPanelAttorney { get; set; }
        public string AttorneyContact { get; set; }
        public int? DeedsOfficeKey { get; set; }
        public string DeedsOfficeDescription { get; set; }
        public decimal? Mandate { get; set; }
        public bool? WorkflowEnabled { get; set; }

        public int? GeneralStatusKey { get; set; }
        public string GeneralStatusDescription { get; set; }
        public int? LegalEntityTypeKey { get; set; }
        public string LegaLEntityStatusDescription { get; set; }

        public string WorkPhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string EmailAddress { get; set; }

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