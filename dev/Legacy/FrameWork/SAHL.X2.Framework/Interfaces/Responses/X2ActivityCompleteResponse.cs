using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ActivityCompleteResponse : X2ResponseBase
    {
        DateTime m_TimeStamp;
        //string m_SessionId;
        Int64 m_InstanceId;
        string m_ActivityName;
        string m_StateName;
        //string m_FormName;
        string m_LinkedActivity;

        public X2ActivityCompleteResponse(DateTime p_TimeStamp, Int64 p_InstanceId, string p_StateName, string p_ActivityName, string p_LinkedActivity, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_TimeStamp = p_TimeStamp;
            m_ActivityName = p_ActivityName;
            m_StateName = p_StateName;
            m_InstanceId = p_InstanceId;
            m_LinkedActivity = p_LinkedActivity;
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

        public string LinkedActivity
        {
            get { return m_LinkedActivity; }
        }
    }
}