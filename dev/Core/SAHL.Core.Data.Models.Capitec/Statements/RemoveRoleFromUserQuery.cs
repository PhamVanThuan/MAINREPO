using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class RemoveRoleFromUserQuery : ISqlStatement<UserRoleDataModel>
    {
        public RemoveRoleFromUserQuery(Guid userId, Guid roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public Guid UserId { get; protected set; }

        public Guid RoleId { get; protected set; }

        public string GetStatement()
        {
            return "DELETE FROM [Capitec].[security].[UserRole] WHERE UserId = @UserId and RoleId = @RoleId";
        }
    }
}