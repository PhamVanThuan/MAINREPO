using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateLoginAndActivityForUserQuery : ISqlStatement<ActivityDataModel>
    {
        public UpdateLoginAndActivityForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[Activity] SET LastLogin = GETDATE(), LastActivity = GETDATE() WHERE UserId = @UserId";
        }
    }
}