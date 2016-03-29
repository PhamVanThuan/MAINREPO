using SAHL.Core.Attributes;
using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [InsertConventionExclude]
    public class InsertLoginAndActivityForUserQuery : ISqlStatement<ActivityDataModel>
    {
        public InsertLoginAndActivityForUserQuery(Guid userId)
        {
            this.Id = CombGuid.Instance.Generate();
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public Guid Id { get; protected set; }

        public string GetStatement()
        {
            return "INSERT INTO [Capitec].[security].[Activity] (Id, UserId, LastLogin, LastActivity) VALUES(@Id, @UserId, GETDATE(), GETDATE()); ";
        }
    }
}