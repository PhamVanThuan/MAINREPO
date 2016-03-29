using SAHL.Core.Identity.Model;
using System.Collections.Generic;

namespace SAHL.Core.Identity
{
    public interface IUserRepository
    {
        IUserDetails ADFindUser(IUserDetails userDetails);

        IUserDetails FindUserRoles(IUserDetails userDetails);

        IEnumerable<string> GetUserCapabilitiesForOrganisationStructure(int userOrganisationStructureKey);

        IEnumerable<OrganisationStructureCapability> GetUserCapabilities(string username);
    }
}