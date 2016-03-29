using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class InvitationHasAlreadyBeenAcceptedException : Exception
    {
        public InvitationHasAlreadyBeenAcceptedException(DateTime acceptedDate)
            : base(string.Format("The invitation has already been accepted on the: {0}.", acceptedDate))
        {
            this.AcceptedDate = acceptedDate;
        }

        public DateTime AcceptedDate { get; protected set; }
    }
}