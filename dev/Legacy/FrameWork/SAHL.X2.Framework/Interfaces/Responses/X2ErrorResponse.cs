using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ErrorResponse : X2ResponseBase
    {
        DateTime m_TimeStamp;
        int m_ErrorCode = -1;
        string m_ErrorSource = "";

        public X2ErrorResponse(DateTime p_TimeStamp, X2ResponseException p_Exception, int p_ErrorCode, string p_Source, string xml)
            : base(xml)
        {
            _Messages = Messages;
            m_Exception = p_Exception;
            m_TimeStamp = p_TimeStamp;
            m_IsErrorResponse = true;
            m_ErrorCode = p_ErrorCode;
            m_ErrorSource = p_Source;
        }

        public DateTime TimeStamp
        {
            get
            {
                return m_TimeStamp;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_ErrorCode;
            }
        }

        public string ErrorSource
        {
            get
            {
                return m_ErrorSource;
            }
        }
    }
}