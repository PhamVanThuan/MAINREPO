using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Refresh_Application_Timeout.OnGetStageTransition
{
    [Subject("Activity => Refresh_Application_Timeout => OnGetStageTransition")]
    internal class when_refresh_application_timeout : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Refresh_Application_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_refresh_application_timeout = () =>
        {
            result.ShouldMatch("Refresh Application Timeout");
        };
    }
}