using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class UsernameDoesNotExistException : Exception
    {
        public UsernameDoesNotExistException(string username)
            : base(string.Format("The username '{0}' does not exist.", username))
        {
            this.Username = username;
        }

        public string Username { get; protected set; }
    }
}