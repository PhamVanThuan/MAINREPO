using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Exceptions
{
    /// <summary>
    /// Exception used when multiple logins occur under the same user name.
    /// </summary>
    public class MultipleLoginException : Exception
    {

        /// <summary>
        /// Constructor. Expects a message parameter.
        /// </summary>
        /// <param name="message"></param>
        public MultipleLoginException(string message)
            : base(message)
        {

        }
    }
}
