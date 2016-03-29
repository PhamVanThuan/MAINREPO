using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Capitec_Decline_Timeout.OnGetActivityTime
{
    [Subject("Activity => Capitec_Decline_Timeout => OnGetActivityTime")]
    internal class when_capitec_decline_timeout : WorkflowSpecApplicationCapture
    {
        private static DateTime expectedDateTime;
        private static DateTime result;

        private Establish context = () =>
        {
            expectedDateTime = DateTime.Now.AddDays(30);
            result = default(DateTime);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Capitec_Decline_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_datetime_30_days_from_now = () =>
        {
            result.Date.ShouldEqual<DateTime>(expectedDateTime.Date);
        };
    }
}