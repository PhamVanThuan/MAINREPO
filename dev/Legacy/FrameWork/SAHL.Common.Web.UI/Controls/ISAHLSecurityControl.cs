using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Common.Web.UI.Controls
{

    /// <summary>
    /// Interface that all SAHL controls that require security must implement.
    /// </summary>
    public interface ISAHLSecurityControl
    {

        /// <summary>
        /// Tag that identifies the security block in the control.  This should be unique 
        /// per object (view/presenter).
        /// </summary>
        string SecurityTag { get; set; }

        /// <summary>
        /// Determines what is done when the security fails.  Usually this should default 
        /// to <see cref="SAHLSecurityDisplayType.Hide">Hide</see>.
        /// </summary>
        SAHLSecurityDisplayType SecurityDisplayType { get; set; }

        /// <summary>
        /// Occurs when the control tries to authenticate i.e. ensure that all security 
        /// restrictions have been passed.
        /// </summary>
        event SAHLSecurityControlEventHandler Authenticate;

        /// <summary>
        /// Gets/sets how the security event is handled.  This will default to Automatic, but 
        /// can be set to Custom to enable custom implementations.
        /// </summary>
        SAHLSecurityHandler SecurityHandler { get; set; }

    }
}
