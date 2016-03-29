using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetUserFromTokenQuery : ISqlStatement<UserDataModel>
    {
        public GetUserFromTokenQuery(Guid token)
        {
            this.SecurityToken = token;
        }

        public Guid SecurityToken { get; protected set; }

        public string GetStatement()
        {
            return "SELECT UserTab.Id, UserTab.Username, UserTab.PasswordHash, UserTab.SecurityStamp, UserTab.IsActive, UserTab.IsLockedOut FROM [Capitec].[security].[User] UserTab inner join [Capitec].[security].[Token] Token on UserTab.Id = Token.UserId WHERE Token.SecurityToken = @SecurityToken";
        }
    }
}