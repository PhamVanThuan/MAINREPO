using System;
using SAHL.X2.Common;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2RequestBase
    {
        protected RequestType m_RequestType;
        protected IX2MessageCollection _Messages = new X2MessageCollection();
        protected bool _IgnoreWarnings = false;
        protected object _data = null;
        protected Guid _requestCorrelationId;

        public RequestType RequestType
        {
            get { return m_RequestType; }
        }

        public IX2MessageCollection DMC { get { if (null == _Messages) _Messages = new X2MessageCollection(); return _Messages; } set { _Messages = value; } }

        public bool IgnoreWarnings { get { return _IgnoreWarnings; } }

        public object Data { get { return _data; } }

        public Guid RequestCorrelationId { get { return _requestCorrelationId; } }

        public X2RequestBase(bool IgnoreWarnings, object Data)
        {
            this._IgnoreWarnings = IgnoreWarnings;
            this._data = Data;
            _Messages = new X2MessageCollection();
            _requestCorrelationId = Guid.NewGuid();
        }
    }
}