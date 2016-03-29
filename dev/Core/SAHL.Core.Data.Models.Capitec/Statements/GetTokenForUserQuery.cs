using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetTokenForUserQuery : ISqlStatement<TokenDataModel>
    {
        public GetTokenForUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, UserId, SecurityToken, IpAddress, IssueDate FROM [Capitec].[security].[Token] WHERE UserId = @UserId";
        }
    }
}