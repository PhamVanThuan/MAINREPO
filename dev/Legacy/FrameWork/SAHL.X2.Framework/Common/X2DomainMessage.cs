using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2.Framework.Common
{
    [Serializable()]
    public class X2DomainMessage : IX2DomainMessage
    {
        protected string _message;
        protected X2DomainMessageType _messageType;

        public X2DomainMessage(string Message, X2DomainMessageType mType)
        {
            _message = Message;
            _messageType = mType;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public X2DomainMessageType MessageType
        {
            get
            {
                return _messageType;
            }
        }
    }
}
