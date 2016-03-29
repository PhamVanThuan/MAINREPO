using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
        [NolockConventionExclude]
    public class FilterCitiesByNameQueryStatement : IServiceQuerySqlStatement<FilterCitiesByNameQuery, FilterCitiesByNameQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 40 cit.[Id] ,CityName + CASE WHEN prov.ProvinceName IS NOT NULL THEN  ' ('+prov.ProvinceName+')' ELSE '' END as CityName
  FROM [Capitec].[geo].[City] cit (NOLOCK)
  LEFT OUTER JOIN [Capitec].[geo].[Province] prov (NOLOCK) ON cit.ProvinceId = prov.Id
  WHERE CityName like @CityNameFilter+'%'";
        }
    }
}