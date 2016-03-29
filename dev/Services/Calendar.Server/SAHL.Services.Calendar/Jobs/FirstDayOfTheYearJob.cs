using Quartz;
using SAHL.Core;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Calendar.Events;
using System;

namespace SAHL.Services.Calendar.Jobs
{
    public class FirstDayOfTheYearJob : IJobConfiguration
    {
        private IEventRaiser eventRaiser;

        public ITrigger GetJobTrigger(IIocContainer iocContainer)
        {
            var jobDataMap = new JobDataMap();
            jobDataMap.Add("eventRaiser", iocContainer.GetInstance<IEventRaiser>());
            ITrigger trigger = TriggerBuilder.Create()
              .StartNow()
              .WithCronSchedule("0 0 0/6 ? * MON-FRI")
              .UsingJobData(jobDataMap)
              .Build();

            return trigger;
        }

        public void Execute(Quartz.IJobExecutionContext context)
        {
            Console.WriteLine("Executing job: FirstDayOfYear");
            var dataMap = context.Trigger.JobDataMap;
            this.eventRaiser = dataMap["eventRaiser"] as IEventRaiser;
            var @event = new FirstDayOfTheYearEvent(DateTime.Now);
            eventRaiser.RaiseEvent(DateTime.Now, @event, 1, 0, new ServiceRequestMetadata());
        }
    }
}
