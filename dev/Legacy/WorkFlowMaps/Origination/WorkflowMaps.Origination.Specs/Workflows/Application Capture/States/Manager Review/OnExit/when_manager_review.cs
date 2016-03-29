using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Manager_Review.OnExit
{
    [Subject("State => Manager_Review => OnExit")]
    internal class when_manager_review : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.Last_State = "test";
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Manager_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Manager Review");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}