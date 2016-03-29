using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Clone.OnGetActivityTime
{
    [Subject("Activity => Create_Clone => OnGetActivityTime")]
    internal class when_create_clone : WorkflowSpecApplicationCapture
    {
        private static DateTime result;

        private Establish context = () =>
        {
            result = DateTime.Now.AddDays(10);
        };

        private Because of = () =>
        {
            result = workflow.GetActivityTime_Create_Clone(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_todays_date = () =>
        {
            result.Date.ShouldEqual<DateTime>(DateTime.Now.Date);
        };
    }
}