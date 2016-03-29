using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace SAHL.Shared.Messages
{
    public enum LogMessageType
    {
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }

    [Serializable]
    public class LogMessage : MessageBase, ILogMessage
    {
		public LogMessage(LogMessageType logMessageType, string application, string message, string methodName, string machineName, string user = "", Dictionary<string, object> parameters = null)
			: base(application, methodName, user, parameters, machineName : machineName)
		{
			this.LogMessageType = logMessageType;
			this.Message = message.Trim();
			this.MethodName = methodName;
		}
        public LogMessage(LogMessageType logMessageType, string application, string message, string methodName, string user = "", Dictionary<string, object> parameters = null)
            : base(application, user, parameters)
        {
            this.LogMessageType = logMessageType;
            this.Message = message.Trim();
            this.MethodName = methodName;
            this.Source = methodName;
        }

        protected LogMessage()
        {
        }

        public virtual LogMessageType LogMessageType { get; protected set; }

        public virtual string MethodName { get; protected set; }

        public virtual string Message { get; protected set; }
    }
}