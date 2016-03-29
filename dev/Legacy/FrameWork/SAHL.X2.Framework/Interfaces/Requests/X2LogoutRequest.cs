using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2LogoutRequest : X2RequestBase
    {
        string m_SessionId;

        public X2LogoutRequest(string p_SessionId)
            : base(false, null)
        {
            m_RequestType = RequestType.LogoutRequest;
            m_SessionId = p_SessionId;
        }

        public string SessionId
        {
            get { return m_SessionId; }
        }
    }
}