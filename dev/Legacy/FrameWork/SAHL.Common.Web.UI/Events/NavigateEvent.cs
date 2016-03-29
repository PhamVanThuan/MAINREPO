using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Events
{

    /// <summary>
    /// Represents the method that will handle a navigation event in the UI.
    /// </summary>
    /// <param name="sender">Where the navigate event was raised (usually a control).</param>
    /// <param name="e">Arguments containing information on where to navigate to.</param>
    public delegate void NavigateEventHandler(Object sender, NavigateEventArgs e);

    /// <summary>
    /// Provides data for navigation events.
    /// </summary>
    public class NavigateEventArgs
    {
        private string _navigateValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigateValue"></param>
        public NavigateEventArgs(string navigateValue)
        {
            _navigateValue = navigateValue;
        }

        /// <summary>
        /// Where to navigate to.
        /// </summary>
        public string NavigateValue
        {
            get
            {
                return _navigateValue;
            }
        }
    }

}
