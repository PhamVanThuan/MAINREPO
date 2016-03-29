using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
	public class GetSalutationsQueryStatement : IServiceQuerySqlStatement<GetSalutationsQuery, GetSalutationsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[Name]
FROM [Capitec].[dbo].[SalutationEnum] where IsActive = 1";
        }
    }
}