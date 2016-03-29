using System;

namespace SAHL.X2.Common
{
    [Serializable()]
    public class X2Message : IX2Message
    {
        protected string _message;
        protected X2MessageType _messageType;

        public X2Message(string Message, X2MessageType mType)
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

        public X2MessageType MessageType
        {
            get
            {
                return _messageType;
            }
        }
    }
}