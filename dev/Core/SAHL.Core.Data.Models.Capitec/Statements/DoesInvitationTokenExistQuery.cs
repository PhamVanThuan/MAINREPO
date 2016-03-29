using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesInvitationTokenExistQuery : ISqlStatement<InvitationDataModel>
    {
        public DoesInvitationTokenExistQuery(Guid invitationToken)
        {
            this.InvitationToken = invitationToken;
        }

        public Guid InvitationToken { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, UserId, InvitationToken, InvitationDate, AcceptedDate FROM [Capitec].[security].[Invitation] WHERE InvitationToken = @InvitationToken";
        }
    }
}