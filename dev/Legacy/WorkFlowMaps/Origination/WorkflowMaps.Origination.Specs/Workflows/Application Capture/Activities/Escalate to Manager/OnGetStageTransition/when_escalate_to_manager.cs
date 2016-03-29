using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Escalate_to_Manager.OnGetStageTransition
{
    [Subject("Activity => Escalate_to_Manager => OnGetStageTransition")]
    internal class when_escalate_to_manager : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Escalate_to_Manager(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_escalate_to_manager = () =>
        {
            result.ShouldEqual("Escalate to Manager");
        };
    }
}