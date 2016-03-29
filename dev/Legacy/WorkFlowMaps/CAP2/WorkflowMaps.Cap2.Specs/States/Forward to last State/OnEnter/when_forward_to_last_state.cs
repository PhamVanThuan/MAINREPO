using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Forward_to_last_State.OnEnter
{
    [Subject("State => Forward_to_last_State => OnEnter")] // AutoGenerated
    internal class when_forward_to_last_state : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Forward_to_last_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}