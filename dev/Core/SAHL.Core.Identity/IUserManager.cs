using SAHL.Core.Identity.Model;
using System.Collections.Generic;
namespace SAHL.Core.Identity
{
    public interface IUserManager
    {
        IUserDetails GetUserDetails(string adUserName);
        IEnumerable<string> GetUserCapabilitiesForOrganisationStructureKey(int userOrganisationStructureKey);
        IEnumerable<OrganisationStructureCapability> GetUserCapabilities(string username);
    }
}