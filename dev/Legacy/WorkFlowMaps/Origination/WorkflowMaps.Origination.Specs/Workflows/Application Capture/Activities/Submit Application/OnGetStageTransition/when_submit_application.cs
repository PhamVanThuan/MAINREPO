using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Submit_Application.OnGetStageTransition
{
    [Subject("Activity => Submit_Application => OnGetStageTransition")]
    internal class when_submit_application : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Submit_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_submit_application = () =>
        {
            result.ShouldMatch("Submit Application");
        };
    }
}