using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetBranchQueryStatement : IServiceQuerySqlStatement<GetBranchQuery, GetBranchQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT bra.Id,bra.BranchName,bra.BranchCode,bra.IsActive,bra.SuburbId,sub.SuburbName + CASE WHEN prov.ProvinceName IS NOT NULL THEN  ' ('+prov.ProvinceName+')' ELSE '' END as SuburbName
FROM [Capitec].[security].[Branch] bra (NOLOCK)
LEFT OUTER JOIN [Capitec].[geo].Suburb sub (NOLOCK) ON bra.SuburbId = sub.Id
LEFT OUTER JOIN [Capitec].[geo].[City] cit (NOLOCK) ON sub.CityId = cit.Id
LEFT OUTER JOIN [Capitec].[geo].[Province] prov (NOLOCK) ON cit.ProvinceId = prov.Id
WHERE bra.Id = @Id";
        }
    }
}