using Machine.Specifications;
using System;

namespace WorkflowMaps.Cap2.Specs.Activities.Completed_Expired.OnGetActivityTime
{
    [Subject("Activity => Completed_Expired => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecCap2
    {
        private static DateTime capExpireDate;
        private static DateTime activityTime;

        private Establish context = () =>
        {
            capExpireDate = new DateTime(2012, 1, 1);
            workflowData.CapExpireDate = capExpireDate;
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Completed_Expired(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_cap_expiry_date = () =>
        {
            activityTime.ShouldEqual(workflowData.CapExpireDate);
        };
    }
}