using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetDeclarationTypesQueryStatement : IServiceQuerySqlStatement<GetDeclarationTypesQuery, GetDeclarationTypesResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[Name]
FROM [Capitec].[dbo].[DeclarationTypeEnum] where IsActive = 1";
        }
    }
}