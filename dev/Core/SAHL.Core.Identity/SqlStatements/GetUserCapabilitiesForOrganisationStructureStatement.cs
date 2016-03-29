using SAHL.Core.Data;

namespace SAHL.Core.Identity
{
    public class GetUserCapabilitiesForOrganisationStructure : ISqlStatement<string>
    {
        
        public int UserOrganisationStructureKey { get; protected set; }

        public GetUserCapabilitiesForOrganisationStructure(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }

        public string GetStatement()
        {
            return @"SELECT 
                        cap.Description
                     FROM
                        [2am].[OrgStruct].[Capability] cap
                     INNER JOIN 
                        [2am].[OrgStruct].[UserOrganisationStructureCapability] uoscap
                     ON 
                        cap.CapabilityKey = uoscap.CapabilityKey
                     INNER JOIN 
                        [2am].[dbo].[UserOrganisationStructure] uos
                     ON 
                        uos.UserOrganisationStructureKey = uoscap.UserOrganisationStructureKey
                     WHERE
                        uos.UserOrganisationStructureKey = @UserOrganisationStructureKey";
        }
    }
}