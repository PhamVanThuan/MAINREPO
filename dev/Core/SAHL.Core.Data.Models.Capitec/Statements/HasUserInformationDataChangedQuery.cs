using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasUserInformationDataChangedQuery : ISqlStatement<UserInformationDataModel>
    {
        public Guid UserId { get; protected set; }

        public string FirstName { get; protected set; }

        public string LastName { get; protected set; }

        public HasUserInformationDataChangedQuery(Guid userId, string firstName, string lastName)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[UserId],[EmailAddress],[FirstName],[LastName]
FROM [Capitec].[security].[UserInformation]
WHERE [UserId] = @UserId AND [FirstName] = @FirstName AND [LastName] = @LastName";
        }
    }
}