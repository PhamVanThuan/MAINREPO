using System;

namespace SAHL.Common.Exceptions
{
    /// <summary>
    /// Exception class invoked when the CBO node is not found.
    /// </summary>
    public class CBONodeNotFoundException : Exception
    {
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public CBONodeNotFoundException()
            : base("Current CBO node of the expected type not found.")
        {
        }

        /// <summary>
        /// Constructor. Expects a message parameter.
        /// </summary>
        /// <param name="message"></param>
        public CBONodeNotFoundException(string message)
            : base(message)
        {
        }
    }
}