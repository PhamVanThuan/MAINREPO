using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class RemoveTokenForUserQuery : ISqlStatement<TokenDataModel>
    {
        public RemoveTokenForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "DELETE FROM [Capitec].[security].[Token] WHERE UserId = @UserId";
        }
    }
}