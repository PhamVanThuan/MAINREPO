using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetProvincesQueryStatement : IServiceQuerySqlStatement<GetProvincesQuery, GetProvincesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT prov.Id,prov.ProvinceName,COUNT(bra.id) as NumberOfBranches,coun.CountryName
FROM [Capitec].[geo].[Province] prov (NOLOCK)
LEFT OUTER JOIN [Capitec].[geo].[Country] coun (NOLOCK) ON coun.Id = prov.CountryId
LEFT OUTER JOIN [Capitec].[geo].[City] cit (NOLOCK) ON cit.ProvinceId = prov.Id
LEFT OUTER JOIN [Capitec].[geo].[Suburb] sub (NOLOCK) ON sub.CityId = cit.Id
LEFT OUTER JOIN [Capitec].[security].[Branch] bra (NOLOCK) ON bra.SuburbId = sub.Id
GROUP BY prov.Id,prov.ProvinceName,coun.CountryName
";
        }
    }
}