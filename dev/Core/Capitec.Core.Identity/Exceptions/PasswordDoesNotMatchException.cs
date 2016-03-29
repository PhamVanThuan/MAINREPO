using System;

namespace Capitec.Core.Identity.Exceptions
{
    public class PasswordDoesNotMatchException : Exception
    {
        public PasswordDoesNotMatchException()
            : base("The password supplied does not match the stored password.")
        {
        }
    }
}