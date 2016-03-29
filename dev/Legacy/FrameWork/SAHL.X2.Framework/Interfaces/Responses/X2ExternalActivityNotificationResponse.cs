using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ExternalActivityNotificationResponse : X2ResponseBase
    {
        DateTime m_TimeStamp;

        public X2ExternalActivityNotificationResponse(DateTime p_TimeStamp, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_TimeStamp = p_TimeStamp;
        }

        public DateTime TimeStamp
        {
            get { return m_TimeStamp; }
        }
    }
}