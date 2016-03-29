using System;

namespace SAHL.Common.Exceptions
{
    public class MissingDomainMessageCollectionException : Exception
    {
        public MissingDomainMessageCollectionException()
            : base("A DomainMessageCollection is required.")
        {
        }
    }
}