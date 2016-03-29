using Quartz;
using SAHL.Core;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using System;
using System.Runtime.CompilerServices;

namespace SAHL.Services.Calendar
{
    public interface ICalendarService : IStartable, IStoppable
    {
    }

    public class CalendarService : HostedService, ICalendarService
    {
        private IIocContainer iocContainer;
        private IScheduler quartzScheduler;
        private IRawLogger rawLogger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        public CalendarService(IIocContainer iocContainer, ISchedulerFactory scheduleFactory, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.iocContainer = iocContainer;
            this.quartzScheduler = scheduleFactory.GetScheduler();
            this.rawLogger = rawLogger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public override void Start()
        {
            this.LogStartupMessage("Starting Calendar Service");
            try
            {
                quartzScheduler.Start();
                var jobs = iocContainer.GetAllInstances<IJobConfiguration>();
                foreach (var registeredJob in jobs)
                {
                    var job = JobBuilder.Create(registeredJob.GetType()).Build();
                    quartzScheduler.ScheduleJob(job, registeredJob.GetJobTrigger(this.iocContainer));
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("Error starting Calendar Service", exception);
            }

        }

        private void LogStartupMessage(string message, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            this.rawLogger.LogStartup(loggerSource.LogLevel, "CalendarService", loggerSource.Name, "System", methodName, message);
        }

        private void LogMessage(string message, Exception runtimeException = null, [CallerMemberName] string methodName = "")
        {
#if DEBUG
            Console.WriteLine("{0}\n{1}", message, runtimeException);
#endif

            if (runtimeException == null)
            {
                rawLogger.LogInfo(loggerSource.LogLevel, "CalendarService", loggerSource.Name, "System", methodName, message);
            }
            else
            {
                rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                                                "CalendarService", methodName,
                                                string.Format("{0}\n{1}", message, runtimeException), runtimeException);
            }
        }

    }
}
