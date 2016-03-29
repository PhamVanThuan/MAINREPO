using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2SystemRequest : X2RequestBase
    {
        protected Int64 m_InstanceId;
        protected string m_ActivityName;
        protected object _data;

        public X2SystemRequest(Int64 InstanceId, string ActivityName, bool IgnoreWarnings, object Data)
            : base(IgnoreWarnings, Data)
        {
            m_RequestType = RequestType.SystemRequest;
            m_InstanceId = InstanceId;
            m_ActivityName = ActivityName;
            this._data = Data;
        }

        public Int64 InstanceId
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        public string ActivityName
        {
            get { return m_ActivityName; }
            set { m_ActivityName = value; }
        }
    }
}