using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
	public class GetEmploymentTypesQueryStatement : IServiceQuerySqlStatement<GetEmploymentTypesQuery, GetEmploymentTypesResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[Name]
FROM [Capitec].[dbo].[EmploymentTypeEnum] where IsActive = 1";
        }
    }
}