using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetCitiesQueryStatement : IServiceQuerySqlStatement<GetCitiesQuery, GetCitiesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT cit.Id,cit.CityName,COUNT(bra.id) as NumberOfBranches
FROM [Capitec].[geo].[City] cit (NOLOCK)
LEFT OUTER JOIN [Capitec].[geo].[Suburb] sub (NOLOCK) ON sub.CityId = cit.Id
LEFT OUTER JOIN [Capitec].[security].[Branch] bra (NOLOCK) ON bra.SuburbId = sub.Id
GROUP BY cit.Id,cit.CityName";
        }
    }
}