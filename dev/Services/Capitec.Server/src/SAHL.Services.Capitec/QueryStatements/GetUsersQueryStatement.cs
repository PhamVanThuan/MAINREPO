using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    [NolockConventionExclude]
    public class GetUsersQueryStatement : IServiceQuerySqlStatement<GetUsersQuery, GetUsersQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT u.Id,ui.FirstName +' '+ ui.LastName as Name
,ui.EmailAddress
,CASE WHEN u.[IsActive] = 0 THEN 'Inactivate' ELSE 'Active' END as [IsActive]
,bra.BranchName
,CONVERT(VARCHAR(10), act.LastActivity, 103) + ' ' + CONVERT(VARCHAR(8), act.LastActivity, 108) as LastActivity
FROM [Capitec].[Security].[User] u (NOLOCK)
LEFT OUTER JOIN [Capitec].[Security].[UserInformation] ui (NOLOCK) ON u.Id = ui.UserId
LEFT OUTER JOIN [Capitec].[Security].[Activity] act (NOLOCK) ON act.UserId = u.Id
LEFT JOIN [Capitec].[security].[UserBranch] usb (NOLOCK) ON u.Id = usb.UserId
LEFT JOIN [Capitec].[security].[Branch] bra (NOLOCK) ON usb.BranchId = bra.Id
ORDER BY LastName,FirstName ASC";
        }
    }
}