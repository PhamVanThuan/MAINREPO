using SAHL.Core.Data;
using SAHL.Core.Identity.Model;

namespace SAHL.Core.Identity
{
    public class GetUserCapabilitiesStatement : ISqlStatement<OrganisationStructureCapability>
    {

        public string Username { get; protected set; }

        public GetUserCapabilitiesStatement(string username)
        {
            this.Username = username;
        }

        public string GetStatement()
        {
            return @"SELECT uos.UserOrganisationStructureKey,cap.[Description] as CapabilityDescription
FROM [2am].[OrgStruct].[Capability] cap
INNER JOIN [2am].[OrgStruct].[UserOrganisationStructureCapability] uoscap ON cap.CapabilityKey = uoscap.CapabilityKey
INNER JOIN [2am].[dbo].[UserOrganisationStructure] uos ON uos.UserOrganisationStructureKey = uoscap.UserOrganisationStructureKey
INNER JOIN [2AM].[dbo].[ADUser] au ON au.ADUserKey = uos.ADUserKey
WHERE au.ADUserName = @Username";
        }
    }
}