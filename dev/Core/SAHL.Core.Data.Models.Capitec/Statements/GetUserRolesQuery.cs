using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetUserRolesQuery : ISqlStatement<UserRoleDataModel>
    {
        public GetUserRolesQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, UserId, RoleId FROM [Capitec].[security].[UserRole] WHERE UserId = @UserId";
        }
    }
}