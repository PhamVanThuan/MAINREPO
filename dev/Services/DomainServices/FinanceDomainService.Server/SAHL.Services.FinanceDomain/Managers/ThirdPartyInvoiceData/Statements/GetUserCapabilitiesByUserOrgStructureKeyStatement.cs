using SAHL.Core.Data;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetUserCapabilitiesByUserOrgStructureKeyStatement : ISqlStatement<string>
    {
        public int UserOrganisationStructureKey { get; protected set; }
        public GetUserCapabilitiesByUserOrgStructureKeyStatement(int userOrganisationStructureKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
        }
        public string GetStatement()
        {
            return @" SELECT c.[Description]
                      FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] uosc
                      JOIN [2AM].[OrgStruct].[Capability] c on uosc.CapabilityKey = c.CapabilityKey
                      WHERE [UserOrganisationStructureKey] = @UserOrganisationStructureKey";
        }
    }
}