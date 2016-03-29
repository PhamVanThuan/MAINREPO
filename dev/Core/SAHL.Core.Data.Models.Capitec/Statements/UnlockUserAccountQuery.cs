using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UnlockUserAccountQuery : ISqlStatement<UserDataModel>
    {
        public Guid UserId { get; protected set; }

        public UnlockUserAccountQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET IsLockedOut = 0 WHERE Id = @UserId";
        }
    }
}