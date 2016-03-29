using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Web.UI.WebControls;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Helper methods for all SAHL controls.
    /// </summary>
    public class ControlHelpers
    {
        private ControlHelpers()
        {
        }

        /// <summary>
        /// Cleans a string for output as a javascript string.
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static string CleanJavaScript(string js)
        {
            return js.Replace("\\", "\\\\").Replace("'", "\\'");
        }

        /// <summary>
        /// Returns control with ID = <c>controlID</c>. Returns null when the control cannot be found.
        /// </summary>
        /// <param name="control">The top level control e.g. Page.</param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control control, string controlID)
        {
            if (control.ID == controlID) return control;
            foreach (Control c in control.Controls)
            {
                Control cntrl = FindControlRecursive(c, controlID);
                if (cntrl != null) return cntrl;
            }
            return null;
        }
    }
}
