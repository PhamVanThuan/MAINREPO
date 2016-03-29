using System;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2.Framework.Common
{
    public class X2Info : IX2Info
    {
        string _sessionID;
        Int64 _instanceID;
        string _activityName;
        string _CurrentState;

        #region IX2Info Members

        public string CurrentState
        {
            get { return _CurrentState; }
            set { _CurrentState = value; }
        }

        public string SessionID
        {
            get { return _sessionID; }
            set { _sessionID = value; }
        }

        public long InstanceID
        {
            get { return _instanceID; }
            set { _instanceID = value; }
        }

        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        #endregion IX2Info Members
    }
}