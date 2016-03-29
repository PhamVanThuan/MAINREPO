using Machine.Specifications;
using System;

namespace WorkflowMaps.Cap2.Specs.Activities.NTU_Offer.OnTimed
{
    [Subject("Activity => NTU_Offer => OnTimed")]
    public class When_getting_activity_time : WorkflowSpecCap2
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
                activityTime = workflow.GetActivityTime_NTU_Offer(instanceData, workflowData, paramsData, messages);
            };

        private It should_return_the_capexpiredate = () =>
            {
                activityTime.ShouldEqual<DateTime>(capExpireDate);
            };
    }
}