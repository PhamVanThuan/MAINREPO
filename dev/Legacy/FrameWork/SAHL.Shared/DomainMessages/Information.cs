using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.DomainMessages
{
    [Serializable()]
    public class Information : DomainMessage
    {
        public Information(string Message, string Details)
            : base(Message, Details)
        {
            _messageType = DomainMessageType.Info;
        }
    }
}