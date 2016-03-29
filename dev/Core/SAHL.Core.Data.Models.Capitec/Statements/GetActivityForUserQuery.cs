using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetActivityForUserQuery : ISqlStatement<ActivityDataModel>
    {
        public GetActivityForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, UserId, LastLogin, LastActivity FROM [Capitec].[security].[Activity] WHERE UserId = @UserId";
        }
    }
}