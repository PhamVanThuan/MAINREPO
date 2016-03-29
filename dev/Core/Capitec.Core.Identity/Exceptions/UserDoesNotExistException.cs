using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(Guid userId)
            : base(string.Format("The userId '{0}' does not exist.", userId))
        {
            this.UserId = userId;
        }

        public Guid UserId { get; protected set; }
    }
}