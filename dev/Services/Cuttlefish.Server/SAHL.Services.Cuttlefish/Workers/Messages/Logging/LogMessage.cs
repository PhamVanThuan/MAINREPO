using Newtonsoft.Json;

namespace SAHL.Shared.Messages
{
    public enum LogMessageType
    {
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }

    public class LogMessage : MessageBase
    {
        [JsonProperty]
        public virtual int LogMessageType { get; protected set; }

        [JsonProperty]
        public virtual string MethodName { get; protected set; }

        [JsonProperty]
        public virtual string Message { get; protected set; }
    }
}