using System;
using System.Runtime.CompilerServices;

using SAHL.Core.Logging;

namespace SAHL.Services.Halo.Server.QueryHandlers
{
    public abstract class QueryHandlerBase
    {
        private readonly IRawLogger rawLogger;
        private readonly ILoggerSource loggerSource;
        private readonly ILoggerAppSource loggerAppSource;

        protected QueryHandlerBase(IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            if (rawLogger == null) { throw new ArgumentNullException("rawLogger"); }
            if (loggerSource == null) { throw new ArgumentNullException("loggerSource"); }
            if (loggerAppSource == null) { throw new ArgumentNullException("loggerAppSource"); }

            this.rawLogger       = rawLogger;
            this.loggerSource    = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        protected void LogMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
            if (runtimeException == null)
            {
                rawLogger.LogInfo(loggerSource.LogLevel, "Halo Service", loggerSource.Name, "System", methodName, message);
            }
            else
            {
                rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                                "Halo Service", methodName,
                                                string.Format("{0}\n{1}", message, runtimeException), runtimeException);
            }
        }
    }
}
