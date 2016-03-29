using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities._45_day_timer.OnGetActivityTime
{
    [Subject("Activity => 45_day_timer => OnGetActivityTime")]
    internal class when_45_day_timer : WorkflowSpecApplicationCapture
    {
        private static DateTime expectedDateTime;
        private static DateTime result;

        private Establish context = () =>
        {
            expectedDateTime = DateTime.Now.AddDays(45);
            result = default(DateTime);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_45_day_timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_datetime_45_days_from_now = () =>
        {
            result.Date.ShouldEqual<DateTime>(expectedDateTime.Date);
        };
    }
}