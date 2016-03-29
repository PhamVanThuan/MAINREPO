using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Manager_Archive.OnGetStageTransition
{
    [Subject("Activity => Manager_Archive => OnGetStageTransition")]
    internal class when_manager_archive : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Manager_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_manager_archive = () =>
        {
            result.ShouldMatch("Manager Archive");
        };
    }
}