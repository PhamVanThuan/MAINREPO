using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesUserIdExistQuery : ISqlStatement<UserDataModel>
    {
        public Guid UserId { get; protected set; }

        public DoesUserIdExistQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public string GetStatement()
        {
            return "SELECT Id, Username, PasswordHash, SecurityStamp, IsActive, IsLockedOut FROM [Capitec].[security].[User] (NOLOCK) WHERE Id = @UserId";
        }
    }
}