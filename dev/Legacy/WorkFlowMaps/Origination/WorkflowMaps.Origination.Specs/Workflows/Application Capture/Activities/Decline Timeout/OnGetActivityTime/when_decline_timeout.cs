using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Decline_Timeout.OnGetActivityTime
{
    [Subject("Activity => Decline_Timeout => OnGetActivityTime")]
    internal class when_decline_timeout : WorkflowSpecApplicationCapture
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.MinValue;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Decline_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_time_30_days_from_now = () =>
        {
            result.ShouldBeCloseTo(DateTime.Now.AddDays(30), new TimeSpan(0, 0, 5));
        };
    }
}