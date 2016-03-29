using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasUserEmailChangedQuery : ISqlStatement<UserInformationDataModel>
    {
        public Guid Id { get; protected set; }

        public string EmailAddress { get; protected set; }

        public HasUserEmailChangedQuery(Guid userId, string emailAddress)
        {
            this.Id = userId;
            this.EmailAddress = emailAddress;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[UserId],[EmailAddress],[FirstName],[LastName]
FROM [Capitec].[security].[UserInformation]
WHERE Id = @Id AND EmailAddress = @EmailAddress";
        }
    }
}