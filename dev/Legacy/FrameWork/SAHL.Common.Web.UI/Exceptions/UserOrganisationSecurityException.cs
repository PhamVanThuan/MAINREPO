using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Security;

namespace SAHL.Common.Web.UI.Exceptions
{
    /// <summary>
    /// Exception used when a user is not part of the UserOrganisationStructure.
    /// </summary>
    public class UserOrganisationSecurityException : Exception
    {
        /// <summary>
        /// Constructor. Expects a message parameter.
        /// </summary>
        /// <param name="message"></param>
        public UserOrganisationSecurityException(string message)
            : base(message)
        {

        }
    }
}
