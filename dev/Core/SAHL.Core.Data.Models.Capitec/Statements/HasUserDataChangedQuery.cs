using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasUserDataChangedQuery : ISqlStatement<UserDataModel>
    {
        public Guid Id { get; protected set; }
        public bool IsActive { get; protected set; }

        public HasUserDataChangedQuery(Guid userId, bool isActive)
        {
            this.Id = userId;
            this.IsActive = isActive;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[Username],[PasswordHash],[SecurityStamp],[IsActive],[IsLockedOut]
FROM [Capitec].[security].[User]
WHERE Id = @Id AND IsActive = @IsActive";
        }
    }
}