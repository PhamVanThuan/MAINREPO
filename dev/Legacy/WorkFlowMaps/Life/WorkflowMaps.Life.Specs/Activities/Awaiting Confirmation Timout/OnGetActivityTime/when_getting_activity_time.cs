using Machine.Specifications;
using System;

namespace WorkflowMaps.Life.Specs.Activities.Awaiting_Confirmation_Timout.OnGetActivityTime
{
    [Subject("Activity => Awaiting_Confirmation_Timout => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecLife
    {
        private static DateTime activityTime;
        private static DateTime expectedTime;
        private static bool result;

        private Establish context = () =>
        {
            expectedTime = DateTime.Now.AddDays(1);
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Awaiting_Confirmation_Timout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_one_day_from_today = () =>
        {
            TimeSpan daydiff = activityTime - expectedTime;
            daydiff.Days.ShouldEqual(0);
        };
    }
}