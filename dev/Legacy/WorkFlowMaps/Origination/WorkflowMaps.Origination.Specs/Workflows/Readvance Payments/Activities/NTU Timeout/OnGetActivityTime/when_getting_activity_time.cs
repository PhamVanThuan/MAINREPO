using Machine.Specifications;
using System;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.NTU_Timeout.OnGetActivityTime
{
    [Subject("Activity => NTU_Final => OnComplete")]
    internal class when_getting_activity_time : WorkflowSpecReadvancePayments
    {
        private static DateTime activityDate;
        private static DateTime expectedDate;

        private Establish context = () =>
        {
            expectedDate = DateTime.Now.AddDays(30);
        };

        private Because of = () =>
        {
            activityDate = workflow.GetActivityTime_NTU_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_thirty_days_from_today = () =>
        {
            activityDate.ShouldBeCloseTo(expectedDate, TimeSpan.FromSeconds(10));
        };
    }
}