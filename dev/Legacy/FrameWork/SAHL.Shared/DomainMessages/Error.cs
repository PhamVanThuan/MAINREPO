using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.DomainMessages
{
    [Serializable()]
    public class Error : DomainMessage
    {
        public Error(string Message, string Details)
            : base(Message, Details)
        {
            _messageType = DomainMessageType.Error;
        }
    }
}