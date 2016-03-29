using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ActivityCancelResponse : X2ResponseBase
    {
        //string m_ClientData;
        DateTime m_TimeStamp;
        Int64 m_InstanceId;
        string m_ActivityName;

        public X2ActivityCancelResponse(DateTime p_TimeStamp, Int64 p_InstanceId, string p_ActivityName, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_TimeStamp = p_TimeStamp;
            m_InstanceId = p_InstanceId;
            m_ActivityName = p_ActivityName;
        }

        public DateTime TimeStamp
        {
            get { return m_TimeStamp; }
        }

        public Int64 InstanceId
        {
            get { return m_InstanceId; }
        }

        public string ActivityName
        {
            get { return m_ActivityName; }
        }
    }
}