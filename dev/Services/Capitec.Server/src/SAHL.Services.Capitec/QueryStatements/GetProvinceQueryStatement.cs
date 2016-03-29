using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetProvinceQueryStatement : IServiceQuerySqlStatement<GetProvinceQuery, GetProvinceQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT Id,SAHLProvinceKey,ProvinceName,CountryId
FROM [Capitec].[geo].[Province]
WHERE Id = @Id";
        }
    }
}