using Machine.Fakes;
using Machine.Specifications;
using Quartz;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Services.Calendar.Jobs;
using SAHL.Services.Interfaces.Calendar.Events;
using System;

namespace SAHL.Services.Calendar.Server.Specs.JobSpec
{
    public class when_executing_1st_day_of_year : WithFakes
    {
        static IEventRaiser eventRaiser;
        static IJobConfiguration job;
        static IJobExecutionContext jobContext;

        Establish context = () =>
        {
            jobContext = An<IJobExecutionContext>();
            eventRaiser = An<IEventRaiser>();
            job = new FirstDayOfTheYearJob();
            JobDataMap dataMap = new JobDataMap();
            dataMap.Add("eventRaiser", eventRaiser);
            jobContext.Trigger.WhenToldTo(x => x.JobDataMap).Return(dataMap);
        };

        Because of = () =>
        {
            job.Execute(jobContext);
        };

        It should_raise_the_first_day_of_month_event = () =>
        {
            eventRaiser.WasToldTo(x =>
                x.RaiseEvent(
                    Param.IsAny<DateTime>(),
                    Param.IsAny<FirstDayOfTheYearEvent>(),
                    1, 0, Param.IsAny<IServiceRequestMetadata>()
                   ));
        };
    }
}
