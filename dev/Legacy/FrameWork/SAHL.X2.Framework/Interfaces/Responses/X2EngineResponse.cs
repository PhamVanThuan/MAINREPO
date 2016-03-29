using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2EngineResponse : X2ResponseBase
    {
        DateTime m_TimeStamp;

        public X2EngineResponse(DateTime p_TimeStamp, string xml)
            : base(xml)
        {
            m_TimeStamp = p_TimeStamp;
        }

        public DateTime TimeStamp
        {
            get { return m_TimeStamp; }
        }

    }
}