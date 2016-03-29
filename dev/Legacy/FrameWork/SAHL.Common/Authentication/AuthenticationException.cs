using System;

namespace SAHL.Common.Authentication
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException()
            : base()
        { }

        public AuthenticationException(string message)
            : base(message)
        { }

        public AuthenticationException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}