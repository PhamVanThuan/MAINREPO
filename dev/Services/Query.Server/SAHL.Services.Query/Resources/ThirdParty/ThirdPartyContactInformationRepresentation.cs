using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Query.Core;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.ThirdParty
{
    public class ThirdPartyContactInformationRepresentation : Representation, IRepresentation
    {
        private ILinkResolver linkResolver;

        public ThirdPartyContactInformationRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public Guid Id { get; set; }
        public int? LegalEntityKey { get; set; }
        public string HomePhoneNumber { get; set; }
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
