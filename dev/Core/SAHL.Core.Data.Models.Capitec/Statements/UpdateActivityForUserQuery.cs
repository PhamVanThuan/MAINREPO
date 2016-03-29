using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateActivityForUserQuery : ISqlStatement<ActivityDataModel>
    {
        public UpdateActivityForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[Activity] SET LastActivity = GETDATE() WHERE UserId = @UserId";
        }
    }
}