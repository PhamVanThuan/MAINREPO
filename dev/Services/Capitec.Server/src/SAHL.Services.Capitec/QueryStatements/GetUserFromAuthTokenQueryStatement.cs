using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class GetUserFromAuthTokenQueryStatement : IServiceQuerySqlStatement<GetUserFromAuthTokenQuery, GetUserFromAuthTokenQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT UserTab.Id, UserTab.Username, UserInfoTab.FirstName, UserInfoTab.LastName, [Capitec].dbo.CommaDelimit(UserRoleTab.RoleId) as Roles
                      FROM [Capitec].[security].[User] UserTab
                      inner join [Capitec].[security].[Token] Token on UserTab.Id = Token.UserId
                      inner join [Capitec].[security].[UserInformation] UserInfoTab on UserTab.Id = UserInfoTab.UserId
					  inner join [Capitec].[security].[UserRole] UserRoleTab on UserRoleTab.UserId = UserTab.Id
                      WHERE Token.SecurityToken = CONVERT(UNIQUEIDENTIFIER, @AuthenticationToken)
					  GROUP BY UserTab.Id, UserTab.Username, UserInfoTab.FirstName, UserInfoTab.LastName";
        }
    }
}