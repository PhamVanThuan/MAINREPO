using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ActivityStartResponse : X2ResponseBase
    {
        DateTime m_TimeStamp;
        Int64 m_InstanceId;
        string m_ActivityName;
        string m_StateName;
        string m_FormName;

        public X2ActivityStartResponse(DateTime p_TimeStamp, Int64 p_InstanceId, string p_StateName, string p_ActivityName, string p_FormName, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_TimeStamp = p_TimeStamp;
            m_StateName = p_StateName;
            m_InstanceId = p_InstanceId;
            m_ActivityName = p_ActivityName;
            m_FormName = p_FormName;
        }

        public DateTime TimeStamp
        {
            get { return m_TimeStamp; }
        }

        public Int64 InstanceId
        {
            get { return m_InstanceId; }
        }

        public string StateName
        {
            get { return m_StateName; }
        }

        public string ActivityName
        {
            get { return m_ActivityName; }
        }

        public string FormName
        {
            get { return m_FormName; }
        }
    }
}