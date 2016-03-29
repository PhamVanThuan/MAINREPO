using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Events
{
    /// <summary>
    /// Represents the method that will handle an event where a single value has changed (e.g. when a combo-box selected 
    /// value changes).
    /// </summary>
    /// <param name="sender">Where the navigate event was raised (usually a control).</param>
    /// <param name="e">The event arguments.</param>
    public delegate void KeyChangedEventHandler(Object sender, KeyChangedEventArgs e);

    /// <summary>
    /// Event argument class for the <see cref="KeyChangedEventHandler"/> delegate.
    /// </summary>
    public class KeyChangedEventArgs : EventArgs 
    {
        private object _key;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        public KeyChangedEventArgs (object key)
        {
            _key = key;
        }

        /// <summary>
        /// Gets the key that was supplied when the event was raised.
        /// </summary>
        public object Key
        {
            get
            {
                return _key;
            }
        }
    }
}
