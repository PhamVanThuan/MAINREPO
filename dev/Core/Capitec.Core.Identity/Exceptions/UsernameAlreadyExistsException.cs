using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string username)
            : base(string.Format("The username {0} has already been used.", username))
        {
            this.Username = username;
        }

        public string Username { get; protected set; }
    }
}