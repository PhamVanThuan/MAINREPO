using System.Collections.Generic;

namespace SAHL.Services.Interfaces.UserProfile.Models
{
    public class GetCapabilitiesForUserOrganisationStructureQueryResult
    {
        public IEnumerable<string> Capabilities { get; protected set; }

        public GetCapabilitiesForUserOrganisationStructureQueryResult(IEnumerable<string> capabilities)
        {
            this.Capabilities = capabilities;
        }
    }
}