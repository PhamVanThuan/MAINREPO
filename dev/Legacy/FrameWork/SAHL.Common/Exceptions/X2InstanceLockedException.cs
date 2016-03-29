using System;
using SAHL.Common.Interfaces;

namespace SAHL.Common.Exceptions
{
    [Serializable]
    public class X2InstanceLockedException : Exception, INotificationException
    {
        public X2InstanceLockedException(string Message)
            : base(Message)
        { }

        #region INotificationException Members

        public string NotificationMessage
        {
            get { return base.Message; }
        }

        #endregion INotificationException Members
    }
}