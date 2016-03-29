using System;
using System.Runtime.Serialization;

namespace SAHL.Core.X2.Exceptions
{
    [Serializable]
    public class RuleCommandException : System.Exception
    {
        public RuleCommandException(SerializationInfo info, StreamingContext context)
        {
        }

        public RuleCommandException()
            : base() { }

        public RuleCommandException(string message)
            : base(message) { }
    }
}