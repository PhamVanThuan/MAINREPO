using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class UpdateUserInformationQuery : ISqlStatement<UserInformationDataModel>
    {
        public Guid UserId { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }

        public UpdateUserInformationQuery(Guid userId, string emailAddress, string firstName, string lastName)
        {
            this.UserId = userId;
            this.EmailAddress = emailAddress;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[security].[UserInformation]
SET [EmailAddress] = @EmailAddress
,[FirstName] = @FirstName
,[LastName] = @LastName
WHERE [UserId] = @UserId";
        }
    }
}