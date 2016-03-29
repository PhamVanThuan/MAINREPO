using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ActivityCancelRequest : X2RequestBase
    {
        string m_SessionId;
        Int64 m_InstanceId;
        string m_ActivityName;

        public X2ActivityCancelRequest(string p_Session, Int64 p_InstanceId, string p_ActivityName)
            : base(false, null)
        {
            m_RequestType = RequestType.ActivityCancelRequest;
            m_SessionId = p_Session;
            m_InstanceId = p_InstanceId;
            m_ActivityName = p_ActivityName;
        }

        public string SessionId
        {
            get { return m_SessionId; }
            set { m_SessionId = value; }
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