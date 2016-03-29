using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetRolesFromUserQuery : ISqlStatement<RoleDataModel>
    {
        public GetRolesFromUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "SELECT r.Id, r.Name FROM [Capitec].[security].[UserRole] ur with(nolock) join [Capitec].[security].[Role] r with(nolock) on r.Id = ur.RoleId WHERE UserId = @UserId";
        }
    }
}