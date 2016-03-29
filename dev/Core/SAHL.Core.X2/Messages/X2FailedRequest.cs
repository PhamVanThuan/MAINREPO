using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.X2.Messages
{
    public class X2FailedRequest : SAHL.Core.Messaging.Shared.IMessage
    {
        public X2FailedRequest(IX2Request x2Request)
        {
            this.Messages = SystemMessageCollection.Empty();
            this.X2Request = x2Request;
        }

        public IX2Request X2Request { get; set; }

        public ISystemMessageCollection Messages { get; set; }

        public Guid Id
        {
            get { return this.X2Request.CorrelationId; }
        }
    }
}