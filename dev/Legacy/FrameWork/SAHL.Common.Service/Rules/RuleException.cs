using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SAHL.Common.Rules.Service
{
    [Serializable]
    public class RuleException : Exception
    {
        public RuleException() : base() { }
        public RuleException(string Message) : base(Message) { }
        public RuleException(string Message, Exception innerException) : base(Message, innerException) { }
        public RuleException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
