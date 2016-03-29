using SAHL.Core.Attributes;
using SAHL.Core.Data;
using OrganisationStructureDataModel = SAHL.Services.Query.Models.OrganisationStructure.OrganisationStructureDataModel;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure.Statements
{
    [NolockConventionExclude]
    public class GetOrganisationStructureStatement : ISqlStatement<OrganisationStructureDataModel>
    {
        public int? Id { get; private set; }

        public GetOrganisationStructureStatement(int id)
        {
            Id = id;
        }

        public string GetStatement()
        {
            return @"
SELECT  org.OrganisationStructureKey,
        org.ParentKey,
        org.Description,
        org.OrganisationTypeKey,
        org.GeneralStatusKey,
        OrganisationType = ort.Description
FROM [2AM].[dbo].[OrganisationStructure] org WITH (NOLOCK)
INNER JOIN [2AM].[dbo].[OrganisationType] ort WITH (NOLOCK) ON org.OrganisationTypeKey = ort.OrganisationTypeKey
WHERE org.OrganisationStructureKey = @Id
";
        }
    }
}
