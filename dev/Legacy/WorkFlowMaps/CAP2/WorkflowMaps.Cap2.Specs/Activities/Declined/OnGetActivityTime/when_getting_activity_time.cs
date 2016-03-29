using Machine.Specifications;
using System;

namespace WorkflowMaps.Cap2.Specs.Activities.Declined.OnGetActivityTime
{
    [Subject("Activity => Declined => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecCap2
    {
        private static DateTime activityTime;
        private static DateTime capExpireDate;

        private Establish context = () =>
        {
            capExpireDate = new DateTime(2005, 1, 1);
            workflowData.CapExpireDate = capExpireDate;
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Declined(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_cap_expiry_date = () =>
        {
            activityTime.ShouldEqual(workflowData.CapExpireDate);
        };
    }
}