using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using Quartz;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Services.Calendar.Jobs;
using System.Collections.Generic;

namespace SAHL.Services.Calendar.Server.Specs.CalendarServiceSpec
{
    public class when_starting_calendar_service : WithFakes
    {
        private static ICalendarService calenderService;
        private static IRawLogger rawLogger;
        private static ILoggerSource logger;
        private static ILoggerAppSource loggerAppSource;
        private static ISchedulerFactory scheduleFactory;
        private static IScheduler quartzScheduler;
        private static IIocContainer iocContainer;

        private Establish context = () =>
         {
             iocContainer = An<IIocContainer>();
             scheduleFactory = An<ISchedulerFactory>();
             logger = An<ILoggerSource>();
             loggerAppSource = An<ILoggerAppSource>();
             rawLogger = An<IRawLogger>();

             quartzScheduler = An<IScheduler>();
             var registeredJobs = new List<IJobConfiguration>();
             registeredJobs.Add(new FirstDayOfMonthJob());
             registeredJobs.Add(new FirstDayOfTheYearJob());
             iocContainer.WhenToldTo(x => x.GetAllInstances<IJobConfiguration>()).Return(registeredJobs);
             scheduleFactory.WhenToldTo(x => x.GetScheduler()).Return(quartzScheduler);

             calenderService = new CalendarService(iocContainer, scheduleFactory, rawLogger, logger, loggerAppSource);
         };

        private Because of = () =>
         {
             calenderService.Start();
         };

        private It should_start_the_quartz_timer = () =>
         {
             quartzScheduler.WasToldTo(x => x.Start());
         };

        private It should_get_the_job_configurations = () =>
         {
             iocContainer.WasToldTo(x => x.GetAllInstances<IJobConfiguration>());
         };

        private It should_schedule_the_job = () =>
         {
             quartzScheduler.WasToldTo(x => x.ScheduleJob(Param.IsAny<IJobDetail>(), Param.IsAny<ITrigger>()));
         };

        private It should_register_the_first_day_of_the_month_job = () =>
         {
             quartzScheduler.WasToldTo(x => x.ScheduleJob(Arg.Is<IJobDetail>(
                 y => y.JobType.ToString().Contains(typeof(FirstDayOfMonthJob).ToString())), Arg.Any<ITrigger>()));
         };

        private It should_register_the_first_day_of_the_year_job = () =>
         {
             quartzScheduler.WasToldTo(x => x.ScheduleJob(Arg.Is<IJobDetail>(
                 y => y.JobType.ToString().Contains(typeof(FirstDayOfTheYearJob).ToString())), Arg.Any<ITrigger>()));
         };
    }
}