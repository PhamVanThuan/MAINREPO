using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ChangeUserPasswordQuery : ISqlStatement<UserDataModel>
    {
        public ChangeUserPasswordQuery(Guid userId, string passwordHash)
        {
            this.UserId = userId;
            this.PasswordHash = passwordHash;
        }

        public Guid UserId { get; protected set; }

        public string PasswordHash { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET PasswordHash = @PasswordHash WHERE Id = @UserId";
        }
    }
}