using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Manager_Submit_Application.OnGetStageTransition
{
    [Subject("Activity => Manager_Submit_Application => OnGetStageTransition")]
    internal class when_manager_submit_application : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Manager_Submit_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_manager_submit_application = () =>
        {
            result.ShouldMatch("Manager Submit Application");
        };
    }
}