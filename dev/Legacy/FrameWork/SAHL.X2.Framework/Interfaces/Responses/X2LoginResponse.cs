using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2LoginResponse : X2ResponseBase
    {
        string m_UserId;
        string m_SessionId;
        DateTime m_SessionStartTime;
        DateTime m_LastActivityTime;

        public X2LoginResponse(string p_UserId, string p_SessionId, DateTime SessionStartTime, DateTime LastActivityTime, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_UserId = p_UserId;
            m_SessionId = p_SessionId;
            m_SessionStartTime = SessionStartTime;
            m_LastActivityTime = LastActivityTime;
        }

        public string UserId
        {
            get { return m_UserId; }
        }

        public string SessionId
        {
            get { return m_SessionId; }
        }

        public DateTime SessionStartTime
        {
            get { return m_SessionStartTime; }
        }

        public DateTime LastActivityTime
        {
            get { return m_LastActivityTime; }
        }
    }
}