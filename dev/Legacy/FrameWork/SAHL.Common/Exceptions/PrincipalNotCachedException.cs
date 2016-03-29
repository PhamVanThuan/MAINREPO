using System;

namespace SAHL.Common.Exceptions
{
    public class PrincipalNotCachedException : Exception
    {
        public PrincipalNotCachedException()
            : base("principal does not exist in cache, cannot continue.")
        {
        }
    }
}