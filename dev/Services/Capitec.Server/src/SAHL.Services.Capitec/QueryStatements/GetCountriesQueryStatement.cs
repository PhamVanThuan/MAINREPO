using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetCountriesQueryStatement : IServiceQuerySqlStatement<GetCountriesQuery, GetCountriesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT [Id],[CountryName]
FROM [Capitec].[geo].[Country]";
        }
    }
}