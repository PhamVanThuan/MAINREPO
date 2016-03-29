using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Events
{

    /// <summary>
    /// Delegate for handling <see cref="SAHL.Common.Web.UI.Controls.ISAHLSecurityControl"/> events.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void SAHLSecurityControlEventHandler(object source, SAHLSecurityControlEventArgs e);

    /// <summary>
    /// Class for holding event arguments for <see cref="SAHL.Common.Web.UI.Controls.ISAHLSecurityControl"/> events.
    /// </summary>
    public class SAHLSecurityControlEventArgs : EventArgs
    {

        private bool _cancel;

        /// <summary>
        /// Used to determine whether authorisation has failed.  Event subscribers 
        /// should set this to true so the <see cref="SAHL.Common.Web.UI.Controls.ISAHLSecurityControl"/> raising 
        /// the event knows that the security check has failed.
        /// </summary>
        public bool Cancel 
        { 
            get 
            { 
                return _cancel; 
            } 
            set 
            { 
                _cancel = value; 
            } 
        }

    }
}
