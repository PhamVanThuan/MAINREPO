using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disbursed_Timer.OnGetActivityTime
{
    [Subject("Activity => Disbursed_Timer => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecPersonalLoans
    {
        private static DateTime activityTime;

        private Establish context = () =>
        {
            activityTime = DateTime.MinValue;
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Disbursed_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_twenty_four_hours_from_now = () =>
        {
            activityTime.ShouldBeCloseTo(DateTime.Now.AddHours(24), TimeSpan.FromSeconds(10));
        };
    }
}