using System;
using System.Collections.Generic;
using SAHL.Common.Interfaces;

namespace SAHL.Common.Exceptions
{
    [Serializable]
    public class X2ActivityStartFailedException : Exception, INotificationException
    {
        protected List<string> _Messages = new List<string>();

        public X2ActivityStartFailedException(string Message, List<string> Messages)
            : base(Message)
        {
            _Messages = Messages;
        }

        public List<string> Messages { get { return _Messages; } }

        #region INotificationException Members

        /// <summary>
        /// Gets the list of exceptions as a single string (split by line breaks).
        /// </summary>
        public string NotificationMessage
        {
            get
            {
                return String.Join(System.Environment.NewLine, _Messages.ToArray());
            }
        }

        #endregion INotificationException Members
    }
}