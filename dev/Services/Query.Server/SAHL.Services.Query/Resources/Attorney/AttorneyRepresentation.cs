using System;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Attorney
{
    public class AttorneyRepresentation : Representation, IRepresentation
    {
        private ILinkResolver linkResolver;

        public AttorneyRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public Guid Id { get; set; }
        public int? AttorneyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string Name { get; set; }
        public bool? IsLitigationAttorney { get; set; }
        public bool? IsRegistrationAttorney { get; set; }
        public bool? IsPanelAttorney { get; set; }
        public string AttorneyContact { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string GeneralStatus { get; set; }
        public int? DeedsOfficeKey { get; set; }
        public string DeedsOffice { get; set; }
        public decimal? Mandate { get; set; }
        public bool? WorkflowEnabled { get; set; }

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
