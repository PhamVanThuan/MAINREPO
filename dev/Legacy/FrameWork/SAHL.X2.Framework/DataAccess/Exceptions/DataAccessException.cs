using System;
using System.Runtime.Serialization;

namespace SAHL.X2.Framework.DataAccess.Exceptions
{
    public class SecurityConfigurationException : Exception, ISerializable
    {
        public SecurityConfigurationException()
            : base()
        {
            // Add implementation.
        }

        public SecurityConfigurationException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public SecurityConfigurationException(string message, Exception inner)
            : base()
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected SecurityConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }

    public class DataAccessException : Exception, ISerializable
    {
        public DataAccessException()
            : base()
        {
            // Add implementation.
        }

        public DataAccessException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public DataAccessException(string message, Exception inner)
            : base()
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}