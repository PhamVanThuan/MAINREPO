using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Common.Web.UI.Controls
{
    public enum EditMode
    {
        ReadOnly,
        Editable
    }
    public enum SelectListPositionEnum
    {
        Bottom,
        Top
    }

    /// <summary>
    /// Identifies display options for security-aware controls when authorisation fails.
    /// </summary>
    /// <see cref="ISAHLSecurityControl"/>
    public enum SAHLSecurityDisplayType
    {
        /// <summary>
        /// The control will be hidden if authentication fails.
        /// </summary>
        Hide,

        /// <summary>
        /// The control will be disabled if authentication fails.
        /// </summary>
        Disable
    }


}
