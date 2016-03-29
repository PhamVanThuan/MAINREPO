using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure.Statements
{
    [NolockConventionExclude]
    public class GetOrganisationTypesStatement : ISqlStatement<OrganisationTypeDataModel>
    {
        public string GetStatement()
        {
            return @"
SELECT  org.OrganisationTypeKey,
        org.Description
FROM [2AM].[dbo].[OrganisationType] org WITH (NOLOCK)
";
        }
    }
}