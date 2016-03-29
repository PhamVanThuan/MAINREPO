using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateTokenForUserQuery : ISqlStatement<TokenDataModel>
    {
        public UpdateTokenForUserQuery(Guid userId, Guid token, string ipAddress)
        {
            this.UserId = userId;
            this.SecurityToken = token;
            this.IPAddress = ipAddress;
        }

        public Guid UserId { get; protected set; }

        public Guid SecurityToken { get; protected set; }

        public string IPAddress { get; protected set; }

        public DateTime IssueDate { get { return DateTime.Now; } }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[Token] SET SecurityToken = @SecurityToken, IpAddress = @IpAddress, IssueDate = @IssueDate WHERE UserId = @UserId";
        }
    }
}