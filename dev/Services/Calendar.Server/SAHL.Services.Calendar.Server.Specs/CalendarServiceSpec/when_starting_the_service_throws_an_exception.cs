using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using Quartz;
using SAHL.Core;
using SAHL.Core.Logging;
using System;

namespace SAHL.Services.Calendar.Server.Specs.CalendarServiceSpec
{
    public class when_starting_the_service_throws_an_exception : WithFakes
    {
        private static Exception ex;
        private static CalendarService calendarService;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static ISchedulerFactory scheduleFactory;
        private static IScheduler quartzScheduler;
        private static IIocContainer iocContainer;

        private Establish context = () =>
        {
            iocContainer = An<IIocContainer>();
            scheduleFactory = An<ISchedulerFactory>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            rawLogger = An<IRawLogger>();
            loggerSource.WhenToldTo(x => x.Name).Return("loggerSourceName");
            loggerSource.WhenToldTo(x => x.LogLevel).Return(LogLevel.StartUp);
            loggerAppSource.WhenToldTo(x => x.ApplicationName).Return("CalendarService");
            ex = new Exception("service did not start");
            quartzScheduler = An<IScheduler>();
            iocContainer.WhenToldTo(x => x.GetAllInstances<IJobConfiguration>()).Throw(ex);
            calendarService = new CalendarService(iocContainer, scheduleFactory, rawLogger, loggerSource, loggerAppSource);
        };

        private Because of = () =>
        {
            calendarService.Start();
        };

        private It should_log_the_exception = () =>
         {
             rawLogger.Received().LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                 "CalendarService", "Start", Arg.Any<string>(), ex);
         };
    }
}