using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetCityQueryStatement : IServiceQuerySqlStatement<GetCityQuery, GetCityQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT Id,SAHLCityKey,CityName,ProvinceId
FROM [Capitec].[geo].[City]
WHERE Id = @Id";
        }
    }
}