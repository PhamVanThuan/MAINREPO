using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.X2.Messages
{
    public class X2ErrorResponse : X2Response
    {
        public X2ErrorResponse(Guid requestID, string message, long? instanceId, SystemMessageCollection systemMessages)
            : base(requestID, message, instanceId ?? 0, true)
        {
            this.SystemMessages = systemMessages;
        }
    }
}