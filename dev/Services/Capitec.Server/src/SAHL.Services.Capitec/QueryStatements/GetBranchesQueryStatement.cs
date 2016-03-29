using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetBranchesQueryStatement : IServiceQuerySqlStatement<GetBranchesQuery, GetBranchesQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT bra.[Id]
    ,[BranchName]
	,ISNULL(sub.SuburbName,'Not Set') as SuburbName
	,ISNULL(prov.ProvinceName,'Not Set') as ProvinceName
	,CASE WHEN [IsActive] = 0 THEN 'Inactive' ELSE 'Active' END as [IsActive]
	,COUNT(usrBra.UserId) as NumberOfUsers
FROM [Capitec].[security].[Branch] bra (NOLOCK)
LEFT JOIN [Capitec].[geo].[Suburb] sub (NOLOCK) ON sub.Id = bra.SuburbId
LEFT JOIN [Capitec].[geo].[City] cit (NOLOCK) ON cit.Id = sub.CityId
LEFT JOIN [Capitec].[geo].[Province] prov (NOLOCK) ON prov.id = cit.ProvinceId
LEFT OUTER JOIN [Capitec].[security].[UserBranch] usrBra (NOLOCK) ON bra.Id = usrBra.BranchId
GROUP BY bra.[Id],[BranchName],sub.SuburbName,prov.ProvinceName,[IsActive]
ORDER BY BranchName ASC";
        }
    }
}