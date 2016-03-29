using SAHL.Core.Configuration;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Messages
{
    [Serializable]
    public class LoggingMessage : BaseMessage, ILoggingMessage
    {
        public LoggingMessage(LogLevel logLevel, string message, string methodName, string user = "", IDictionary<string, object> parameters = null)
            : this(logLevel, ApplicationConfigurationProvider.Instance.ApplicationName, message, methodName, user, parameters)
        {
        }

        public LoggingMessage(LogLevel logLevel, string application, string message, string methodName, string user = "", IDictionary<string, object> parameters = null)
            : base(methodName, user, parameters)
        {
            this.LogLevel = logLevel.ToString();
            this.Message = message;
        }

        public string Message { get; protected set; }

        public string LogLevel { get; protected set; }
    }
}