using System;
using System.Xml;
using SAHL.X2.Common;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ResponseBase
    {
        protected string _xmlResponse;
        protected X2ResponseException m_Exception;
        protected bool m_IsErrorResponse = false;
        protected IX2MessageCollection _Messages = new X2MessageCollection();
        protected Guid _requestCorrelationId;

        public X2ResponseException Exception
        {
            get { return m_Exception; }
            set { m_Exception = value; }
        }

        public XmlDocument XMLResponse
        {
            get
            {
                if (string.Empty != _xmlResponse)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(_xmlResponse);
                    return xd;
                }
                return null;
            }
        }

        public bool IsErrorResponse
        {
            get
            {
                return m_IsErrorResponse;
            }
        }

        public Guid RequestCorrelationId
        {
            get { return _requestCorrelationId; }
            set { _requestCorrelationId = value; }
        }

        public X2ResponseBase(string xml)
        {
            this._xmlResponse = xml;
            _Messages = new X2MessageCollection();
        }

        public IX2MessageCollection Messages { get { return _Messages; } set { _Messages = value; } }
    }
}