using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Archive_Application.OnGetActivityTime
{
    [Subject("Activity => Archive_Application => OnGetActivityTime")]
    internal class when_archive_application : WorkflowSpecApplicationCapture
    {
        private static DateTime result;
        private static DateTime expectedResult;

        private Establish context = () =>
        {
            result = default(DateTime);
            expectedResult = DateTime.Now.AddDays(30);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Archive_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_date_30_days_from_now = () =>
        {
            result.Date.ShouldEqual<DateTime>(expectedResult.Date);
        };
    }
}