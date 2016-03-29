using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetRoleTypesQueryStatement : IServiceQuerySqlStatement<GetRoleTypesQuery, GetRoleTypesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[Name]
FROM [Capitec].[security].[Role]";
        }
    }
}