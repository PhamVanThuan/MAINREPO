using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetUserFromUsernameQuery : ISqlStatement<UserDataModel>
    {
        public string Username { get; protected set; }

        public GetUserFromUsernameQuery(string username)
        {
            this.Username = username;
        }

        public string GetStatement()
        {
            return "SELECT Id, Username, PasswordHash, SecurityStamp, IsActive, IsLockedOut FROM [Capitec].[security].[User]  (NOLOCK) WHERE Username = @Username";
        }
    }
}