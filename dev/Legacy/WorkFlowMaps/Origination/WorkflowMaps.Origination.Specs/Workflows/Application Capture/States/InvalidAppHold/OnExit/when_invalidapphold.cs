using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.InvalidAppHold.OnExit
{
    [Subject("State => InvalidAppHold => OnExit")]
    internal class when_invalidapphold : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_InvalidAppHold(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("InvalidAppHold");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}