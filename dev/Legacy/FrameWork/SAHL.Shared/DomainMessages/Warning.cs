using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.DomainMessages
{
    [Serializable()]
    public class Warning : DomainMessage
    {
        public Warning(string Message, string Details)
            : base(Message, Details)
        {
            _messageType = DomainMessageType.Warning;
        }
    }
}