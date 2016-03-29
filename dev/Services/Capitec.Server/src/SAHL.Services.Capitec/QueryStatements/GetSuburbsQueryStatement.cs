using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetSuburbsQueryStatement : IServiceQuerySqlStatement<GetSuburbsQuery, GetSuburbsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT sub.Id,SuburbName,ProvinceName,Count(bra.Id) as NumberOfBranches
FROM [Capitec].[geo].Suburb sub (NOLOCK)
JOIN [Capitec].[geo].City cit (NOLOCK) ON cit.Id = sub.CityId
JOIN [Capitec].[geo].Province prov (NOLOCK) ON prov.Id = cit.ProvinceId
LEFT OUTER JOIN [Capitec].[security].Branch bra (NOLOCK) ON bra.SuburbId = sub.Id
GROUP BY sub.Id,SuburbName,ProvinceName
ORDER BY SuburbName ASC";
        }
    }
}