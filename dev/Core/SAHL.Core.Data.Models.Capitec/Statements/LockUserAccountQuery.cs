using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class LockUserAccountQuery : ISqlStatement<UserDataModel>
    {
        public Guid UserId { get; protected set; }

        public LockUserAccountQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET IsLockedOut = 1 WHERE Id = @UserId";
        }
    }
}