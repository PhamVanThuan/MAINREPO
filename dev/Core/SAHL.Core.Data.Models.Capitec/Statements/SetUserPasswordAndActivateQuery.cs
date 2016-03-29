using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class SetUserPasswordAndActivateQuery : ISqlStatement<UserDataModel>
    {
        public SetUserPasswordAndActivateQuery(Guid userId, string passwordHash)
        {
            this.UserId = userId;
            this.PasswordHash = passwordHash;
        }

        public Guid UserId { get; protected set; }

        public string PasswordHash { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET PasswordHash = @PasswordHash, IsActive = 1 WHERE Id = @UserId";
        }
    }
}