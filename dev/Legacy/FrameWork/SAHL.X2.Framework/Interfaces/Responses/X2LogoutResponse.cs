using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2LogoutResponse : X2ResponseBase
    {
        public X2LogoutResponse(string xml) : base(xml) { }

        DateTime m_TimeStamp;

        public X2LogoutResponse(DateTime p_TimeStamp, string xml)
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