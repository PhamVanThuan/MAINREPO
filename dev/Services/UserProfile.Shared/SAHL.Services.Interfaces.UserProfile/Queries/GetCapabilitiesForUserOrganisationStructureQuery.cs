using SAHL.Core.Services;
using SAHL.Services.Interfaces.UserProfile.Models;

namespace SAHL.Services.Interfaces.UserProfile.Queries
{
    public class GetCapabilitiesForUserOrganisationStructureQuery : ServiceQuery<GetCapabilitiesForUserOrganisationStructureQueryResult>
    {
        public int UserOrganisationStructureKey { get; protected set; }

        public GetCapabilitiesForUserOrganisationStructureQuery(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }
    }
}