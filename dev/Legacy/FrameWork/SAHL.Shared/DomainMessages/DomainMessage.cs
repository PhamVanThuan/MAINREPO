using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.DomainMessages
{
    [Serializable()]
    public class DomainMessage : IDomainMessage
    {
        protected string _message;
        protected string _details;
        protected DomainMessageType _messageType;

        public DomainMessage(string Message, string Details)
        {
            _message = Message;
            _details = Details;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public string Details
        {
            get
            {
                return _details;
            }
        }

        public DomainMessageType MessageType
        {
            get
            {
                return _messageType;
            }
        }
    }
}