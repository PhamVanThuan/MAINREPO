using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class AcceptInvitationTokenQuery : ISqlStatement<InvitationDataModel>
    {
        public AcceptInvitationTokenQuery(Guid invitationToken)
        {
            this.InvitationToken = invitationToken;
        }

        public Guid InvitationToken { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[security].[Invitation] SET AcceptedDate = GETDATE() WHERE InvitationToken = @InvitationToken";
        }
    }
}