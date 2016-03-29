using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities._15_Day_Auto_Reminder.OnGetActivityTime
{
    [Subject("Activity => 15_Day_Auto_Reminder => OnGetActivityTime")]
    internal class when_15_day_auto_reminder : WorkflowSpecApplicationCapture
    {
        private static DateTime expectedDateTime;
        private static DateTime result;

        private Establish context = () =>
        {
            expectedDateTime = DateTime.Now.AddDays(15);
            result = default(DateTime);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_15_Day_Auto_Reminder(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_datetime_15_days_from_now = () =>
        {
            result.Date.ShouldEqual<DateTime>(expectedDateTime.Date);
        };
    }
}