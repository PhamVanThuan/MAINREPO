using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateUserQuery : ISqlStatement<UserDataModel>
    {
        public Guid Id { get; protected set; }

        public bool IsActive { get; protected set; }

        public UpdateUserQuery(Guid userId, bool isActive)
        {
            this.Id = userId;
            this.IsActive = isActive;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[security].[User]
SET IsActive = @IsActive
WHERE Id = @Id";
        }
    }
}