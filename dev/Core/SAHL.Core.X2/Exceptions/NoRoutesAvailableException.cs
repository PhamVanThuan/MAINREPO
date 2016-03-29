using System;
using System.Runtime.Serialization;

namespace SAHL.Core.X2.Exceptions
{
    [Serializable]
    public class NoRoutesAvailableException : Exception
    {
        public NoRoutesAvailableException(SerializationInfo info, StreamingContext context)
        {
        }

        public NoRoutesAvailableException(string message)
            : base(message)
        {
        }
    }
}