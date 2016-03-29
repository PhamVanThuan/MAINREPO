using System;
using System.Runtime.Serialization;

namespace SAHL.Common.DataAccess.Exceptions
{
    public class MetricException : Exception, ISerializable
    {
        public MetricException()
            : base()
        {
            // Add implementation.
        }

        public MetricException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public MetricException(string message, Exception inner)
            : base()
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected MetricException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}