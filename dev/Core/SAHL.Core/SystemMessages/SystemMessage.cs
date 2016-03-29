using System;

namespace SAHL.Core.SystemMessages
{
    [Serializable]
    public class SystemMessage : SAHL.Core.SystemMessages.ISystemMessage
    {
        public SystemMessage(string message, SystemMessageSeverityEnum severity)
        {
            this.Message = message;
            this.Severity = severity;
        }

        public SystemMessageSeverityEnum Severity { get; protected set; }

        public string Message { get; protected set; }
    }
}