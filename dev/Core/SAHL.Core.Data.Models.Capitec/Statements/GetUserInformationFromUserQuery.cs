using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetUserInformationFromUserQuery : ISqlStatement<UserInformationDataModel>
    {
        public GetUserInformationFromUserQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, UserId, EmailAddress, FirstName, LastName FROM [Capitec].[security].[UserInformation] with(nolock) WHERE UserId = @userId";
        }
    }
}