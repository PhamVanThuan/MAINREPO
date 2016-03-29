using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers.Statements
{
    public class GetCapabilitiesForUserOrganisationStructureKeyStatement : ISqlStatement<CapabilityDataModel>
    {
        public int UserOrganisationStructureKey { get; private set; }

        public GetCapabilitiesForUserOrganisationStructureKeyStatement(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }

        public string GetStatement()
        {
            return @"SELECT cap.CapabilityKey, cap.Description
                    FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] uosc
                    INNER JOIN [2AM].[OrgStruct].[Capability] cap ON cap.CapabilityKey = uosc.CapabilityKey
                    WHERE uosc.UserOrganisationStructureKey = @UserOrganisationStructureKey
                    ";
        }
    }
}