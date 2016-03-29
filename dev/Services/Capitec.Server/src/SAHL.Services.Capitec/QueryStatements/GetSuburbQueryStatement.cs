using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
        [NolockConventionExclude]
    public class GetSuburbQueryStatement : IServiceQuerySqlStatement<GetSuburbQuery, GetSuburbQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT sub.Id,sub.SAHLSuburbKey,sub.SuburbName,sub.PostalCode,sub.CityId,CityName + CASE WHEN prov.ProvinceName IS NOT NULL THEN  ' ('+prov.ProvinceName+')' ELSE '' END as CityName
FROM [Capitec].[geo].[Suburb] sub (NOLOCK)
JOIN [Capitec].[geo].[City] cit (NOLOCK) ON sub.CityId = cit.Id
LEFT OUTER JOIN [Capitec].[geo].[Province] prov (NOLOCK) ON cit.ProvinceId = prov.Id
WHERE sub.Id = @Id";
        }
    }
}