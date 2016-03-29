using SAHL.Core.Attributes;
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [InsertConventionExclude]
    public class InsertTokenForUserQuery : ISqlStatement<TokenDataModel>
    {
        public InsertTokenForUserQuery(Guid userId, Guid token, string ipAddress)
        {
            this.Id = CombGuid.Instance.Generate();
            this.UserId = userId;
            this.SecurityToken = token;
            this.IPAddress = ipAddress;
        }

        public Guid Id { get; protected set; }

        public Guid UserId { get; protected set; }

        public Guid SecurityToken { get; protected set; }

        public string IPAddress { get; protected set; }

        public DateTime IssueDate { get { return DateTime.Now; } }

        public string GetStatement()
        {
            return "INSERT INTO [Capitec].[security].[Token] (Id, UserId, SecurityToken, IpAddress, IssueDate) VALUES(@Id, @UserId, @SecurityToken, @IpAddress, @IssueDate); ";
        }
    }
}