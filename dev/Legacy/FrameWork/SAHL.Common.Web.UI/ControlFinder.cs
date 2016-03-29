using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace SAHL.Common.Web.UI
{
    public static class ControlFinder
    {
        /// <summary>
        /// Similar to Control.FindControl, but recurses through child controls.
        /// </summary>
        public static T FindControl<T>(Control startingControl, string id) where T : Control
        {
            if (startingControl == null)
            {
                throw new Exception("startControl is null");
            }
            T found = startingControl.FindControl(id) as T;

            if (found == null)
            {
                found = FindChildControl<T>(startingControl, id);
            }

            return found;
        }

        /// <summary>     
        /// Similar to Control.FindControl, but recurses through child controls.
        /// Assumes that startingControl is NOT the control you are searching for.
        /// </summary>
        public static T FindChildControl<T>(Control startingControl, string id) where T : Control
        {
            T found = null;
            if (startingControl == null)
            {
                throw new Exception("startControl is null");
            }
            foreach (Control activeControl in startingControl.Controls)
            {
                found = activeControl as T;

                if (found == null || (string.Compare(id, found.ID, true) != 0))
                {
                    found = FindChildControl<T>(activeControl, id);
                }

                if (found != null)
                {
                    break;
                }
            }

            return found;
        }
    }
}
