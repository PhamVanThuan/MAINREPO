using Machine.Specifications;
using System;

namespace WorkflowMaps.Life.Specs.Activities.Archive_NTU.OnGetActivityTime
{
    [Subject("Activity => Archive_NTU => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecLife
    {
        private static DateTime activityTime;
        private static DateTime expectedTime;

        private Establish context = () =>
        {
            expectedTime = DateTime.Now.AddDays(45);
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Archive_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_get_45_days_from_today = () =>
        {
            activityTime.ShouldBeCloseTo(activityTime, new TimeSpan(0, 0, 5));
        };
    }
}