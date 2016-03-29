using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DeactivateUserQuery : ISqlStatement<UserDataModel>
    {
        public Guid UserId { get; protected set; }

        public DeactivateUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[User] SET IsActive = 0 WHERE Id = @UserId";
        }
    }
}