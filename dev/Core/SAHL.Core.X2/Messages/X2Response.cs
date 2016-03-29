using System;

namespace SAHL.Core.X2.Messages
{
    public class X2Response : IX2Message
    {
        public X2Response(Guid requestID, string message, long instanceId, bool isErrorResponse = false)
        {
            this.RequestID = requestID;
            this.Message = message;
            this.InstanceId = instanceId;
            this.IsErrorResponse = isErrorResponse;
        }

        public Guid Id { get; set; }

        public Guid RequestID { get; set; }

        public long InstanceId { get; set; }

        public string Message { get; set; }

        public bool IsErrorResponse { get; protected set; }

        public SystemMessages.SystemMessageCollection SystemMessages
        {
            get;
            set;
        }

        public X2Response Result
        {
            get;
            set;
        }
    }
}