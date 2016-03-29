using System;

namespace SAHL.X2.Framework.Common
{
    [Serializable]
    public class X2Params : IX2Params
    {
        string _StateName;
        string _ActivityName;
        string _ADUSerName;
        bool _IgnoreWarnings;
        object _Data;

        public X2Params(string StateName, string ActivityName, string ADUSerName, bool IgnoreWarnings, object Data)
        {
            this._ADUSerName = ADUSerName;
            this._ActivityName = ActivityName;
            this._Data = Data;
            this._IgnoreWarnings = IgnoreWarnings;
            this._StateName = StateName;
        }

        public string StateName { get { return _StateName; } }

        public string ActivityName { get { return _ActivityName; } }

        public string ADUserName { get { return _ADUSerName; } }

        public bool IgnoreWarning { get { return _IgnoreWarnings; } }

        public object Data { get { return _Data; } }
    }
}