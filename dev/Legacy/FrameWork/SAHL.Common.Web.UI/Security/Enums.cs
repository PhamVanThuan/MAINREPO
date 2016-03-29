using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Security
{
    /// <summary>
    /// Enumeration of possible security types supported in HALO views and presenters.
    /// </summary>
    public enum SecurityTypes
    {
        /// <summary>
        /// Active Directory user.
        /// </summary>
        ADUser,

        /// <summary>
        /// Active Directory group.
        /// </summary>
        ADGroup,

        /// <summary>
        /// HALO feature.
        /// </summary>
        Feature
    }

    /// <summary>
    /// Identifies how the security is handled by the control.
    /// </summary>
    public enum SAHLSecurityHandler
    {
        /// <summary>
        /// Authentication is automatic, and the control will be hidden/disabled according to the SAHLSecurityDisplayType.
        /// </summary>
        Automatic,

        /// <summary>
        /// Authentication of the control is handled in a custom way.
        /// </summary>
        Custom
    }
}
