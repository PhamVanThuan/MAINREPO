using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure.Statements
{
    [NolockConventionExclude]
    public class GetOrganisationTypeStatement : ISqlStatement<OrganisationTypeDataModel>
    {
        public int Id { get; private set; }

        public GetOrganisationTypeStatement(int id)
        {
            Id = id;
        }

        public string GetStatement()
        {
            return @"
SELECT  org.OrganisationTypeKey,
        org.Description
FROM [2AM].[dbo].[OrganisationType] org WITH (NOLOCK)
WHERE org.OrganisationTypeKey = @Id
";
        }
    }
}