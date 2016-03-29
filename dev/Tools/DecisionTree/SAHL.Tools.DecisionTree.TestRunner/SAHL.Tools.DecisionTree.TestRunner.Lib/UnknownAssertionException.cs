using System;
using System.Runtime.Serialization;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions
{
    [Serializable]
    internal class UnknownAssertionException : Exception
    {
        public UnknownAssertionException()
        {
        }

        public UnknownAssertionException(string message) : base(message)
        {
        }

        public UnknownAssertionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}