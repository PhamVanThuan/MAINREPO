using Machine.Fakes;
using Machine.Specifications;
using Quartz;
using Quartz.Impl.Triggers;
using SAHL.Core;
using SAHL.Services.Calendar.Jobs;

namespace SAHL.Services.Calendar.Server.Specs.JobSpec.FirstDayOfMonthJobSpecs
{
    public class when_setting_up_the_job_trigger : WithFakes
    {
        private static ITrigger trigger;
        private static FirstDayOfMonthJob job;
        private static IIocContainer container;

        private Establish context = () =>
         {
             container = An<IIocContainer>();
             job = new FirstDayOfMonthJob();
         };

        private Because of = () =>
         {
             trigger = job.GetJobTrigger(container);
         };

        private It should_use_the_correct_cron_expression = () =>
         {
             ((CronTriggerImpl)trigger).CronExpressionString.ShouldEqual("0 0 0/6 ? * MON-FRI");
         };
    }
}