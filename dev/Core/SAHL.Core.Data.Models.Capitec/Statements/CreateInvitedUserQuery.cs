using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class CreateInvitedUserQuery : ISqlStatement<UserDataModel>
    {
        public string Username { get; protected set; }

        public Guid Id { get; protected set; }

        public Guid SecurityStamp { get; protected set; }

        public CreateInvitedUserQuery(string username)
        {
            this.Username = username;
            this.Id = CombGuid.Instance.Generate();
            this.SecurityStamp = CombGuid.Instance.Generate();
        }

        public string GetStatement()
        {
            return "INSERT INTO [Capitec].[security].[User] (Id, Username, PasswordHash, SecurityStamp, IsActive, IsLockedOut) VALUES(@Id, @Username, '', @SecurityStamp, 0, 0)";
        }
    }
}