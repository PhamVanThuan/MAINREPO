using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using SAHL.Core.IoC;
using SAHL.Core.Logging;

namespace SAHL.Services.DomainProcessManager.Services
{
    public abstract class DomainProcessServiceBase : IStartable, IStoppable
    {
        private readonly IRawLogger rawLogger;
        private readonly ILoggerSource loggerSource;
        private readonly ILoggerAppSource loggerAppSource;

        protected DomainProcessServiceBase(IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            if (rawLogger == null) { throw new ArgumentNullException("rawLogger"); }
            if (loggerSource == null) { throw new ArgumentNullException("loggerSource"); }
            if (loggerAppSource == null) { throw new ArgumentNullException("loggerAppSource"); }

            this.rawLogger = rawLogger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public abstract void Start();

        public abstract void Stop();

        protected void LogStartupMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine("{0}\n{1}", message, runtimeException);
#endif

            if (runtimeException == null)
            {
                this.rawLogger.LogStartup(loggerSource.LogLevel, loggerSource.Name, "Domain Process Manager", "System", methodName, message);
            }
            else
            {
                this.rawLogger.LogStartup(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                          "Domain Process Manager", methodName,
                                          string.Format("{0}\n{1}", message, runtimeException));
            }
        }

        protected void LogMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine("{0}\n{1}", message, runtimeException);
#endif

            if (runtimeException == null)
            {
                rawLogger.LogInfo(loggerSource.LogLevel, loggerSource.Name, "Domain Process Manager", "System", methodName, message);
            }
            else
            {
                // TODO: Remove the string formatting
                var errorWithException = string.Format("{0}\n{1}", message, runtimeException);
                rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                                "Domain Process Manager", methodName,
                                                errorWithException, runtimeException);
            }
        }
    }
}
