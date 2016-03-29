using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class InvitationDoesNotExistException : Exception
    {
        public InvitationDoesNotExistException(Guid invitationToken)
            : base(string.Format("The invitation token {0} does not exist.", invitationToken))
        {
            this.InvitationToken = invitationToken;
        }

        public Guid InvitationToken { get; protected set; }
    }
}