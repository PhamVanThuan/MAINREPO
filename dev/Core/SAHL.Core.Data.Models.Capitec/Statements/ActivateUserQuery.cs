using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ActivateUserQuery : ISqlStatement<UserDataModel>
    {
        public Guid UserId { get; protected set; }

        public ActivateUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET IsActive = 1 WHERE Id = @UserId";
        }
    }
}