using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetRoleByName : ISqlStatement<RoleDataModel>
    {
        public string UserRole { get; protected set; }
        public GetRoleByName(string userRole)
        {
            UserRole = userRole;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[Name] FROM [Capitec].[security].[Role] with(nolock) WHERE name = @UserRole";
        }
    }
}