using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Services.Query.Models.OrganisationStructure;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure.Statements
{
    [NolockConventionExclude]
    public class GetOrganisationStructureByParentKeyStatement : ISqlStatement<OrganisationStructureDataModel>
    {
        public int? ParentKey { get; private set; }

        public GetOrganisationStructureByParentKeyStatement(int? parentKey)
        {
            ParentKey = parentKey;
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
WHERE (@ParentKey IS NULL AND ParentKey IS NULL)
OR ParentKey = @ParentKey
";
        }
    }
}
