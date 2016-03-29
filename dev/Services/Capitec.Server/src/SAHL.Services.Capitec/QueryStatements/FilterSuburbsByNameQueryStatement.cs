using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class FilterSuburbsByNameQueryStatement : IServiceQuerySqlStatement<FilterSuburbsByNameQuery, FilterSuburbsByNameQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 30 sub.[Id] ,sub.SuburbName + CASE WHEN prov.ProvinceName IS NOT NULL THEN  ' ('+prov.ProvinceName+')' ELSE '' END as SuburbName
FROM [Capitec].[geo].Suburb sub
LEFT OUTER JOIN [Capitec].[geo].[City] cit (NOLOCK) ON sub.CityId = cit.Id
LEFT OUTER JOIN [Capitec].[geo].[Province] prov (NOLOCK) ON cit.ProvinceId = prov.Id
WHERE sub.SuburbName like @SuburbNameFilter+'%'
ORDER BY sub.SuburbName";
        }
    }
}