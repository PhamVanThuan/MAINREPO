using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.PollingManager;

namespace SAHL.Services.PollingManager
{
    public interface IPollingManagerService : IStartable, IStoppable
    {
    }

    public class PollingManagerService : HostedService, IPollingManagerService
    {
        private IEnumerable<IPolledHandler> polledHandlers;
        private IRawLogger rawLogger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        public PollingManagerService(IEnumerable<IPolledHandler> polledHandlers, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.rawLogger = rawLogger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
            this.polledHandlers = polledHandlers;
            foreach (IPolledHandler thisPolledHandler in this.polledHandlers)
            {
                thisPolledHandler.Initialise();
            }
        }

        public override void Start()
        {
            this.LogStartupMessage("Starting Polling Manager");

            try
            {
                base.Start();
                foreach (IPolledHandler polledHandler in this.polledHandlers)
                {
                    polledHandler.Start();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("Error starting Coordination Service", exception);
            }
        }

        public override void Stop()
        {
            this.LogMessage("Stopping Polling Manager");
            base.Stop();
            foreach (IPolledHandler thisPolledHandler in this.polledHandlers)
            {
                thisPolledHandler.Stop();
            }
        }

        private void LogStartupMessage(string message, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            this.rawLogger.LogStartup(loggerSource.LogLevel, "Polling Manager", loggerSource.Name, "System", methodName, message);
        }

        private void LogMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine("{0}\n{1}", message, runtimeException);
#endif

            if (runtimeException == null)
            {
                rawLogger.LogInfo(loggerSource.LogLevel, "Polling Manager", loggerSource.Name, "System", methodName, message);
            }
            else
            {
                // TODO: Remove the string formatting
                rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                                "Polling Manager", methodName,
                                                string.Format("{0}\n{1}", message, runtimeException), runtimeException);
            }
        }
    }
}