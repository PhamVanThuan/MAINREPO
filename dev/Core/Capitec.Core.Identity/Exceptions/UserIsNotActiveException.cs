using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class UserIsNotActiveException : Exception
    {
        public UserIsNotActiveException(string username)
            : base(string.Format("The username '{0}' is currently marked as not active and cannot be logged in.", username))
        {
            this.Username = username;
        }

        public string Username { get; protected set; }
    }
}