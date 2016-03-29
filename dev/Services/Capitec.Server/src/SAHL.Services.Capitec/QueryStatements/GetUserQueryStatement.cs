using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetUserQueryStatement : IServiceQuerySqlStatement<GetUserQuery, GetUserQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT u.Id,u.Username,u.IsActive,ui.EmailAddress,ui.FirstName,ui.LastName,b.BranchName,ub.BranchId,[Capitec].dbo.CommaDelimit(ur.RoleId) as Roles
FROM [Capitec].[security].[User] u (NOLOCK)
LEFT OUTER JOIN [Capitec].[security].[UserInformation] ui (NOLOCK) ON u.Id = ui.UserId
LEFT OUTER JOIN [Capitec].[security].[UserRole] ur on ur.UserId = u.Id
LEFT OUTER JOIN [Capitec].[security].[UserBranch] ub on ub.UserId = u.Id
LEFT OUTER JOIN [Capitec].[security].[Branch] b on b.Id = ub.BranchId
WHERE u.Id = @Id
GROUP BY u.Id,u.Username,u.IsActive,ui.EmailAddress,ui.FirstName,ui.LastName,b.BranchName,ub.BranchId";
        }
    }
}