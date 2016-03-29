using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Forward_to_last_State.OnAutoForward
{
    [Subject("State => Forward_to_last_State => OnAutoForward")]
    internal class when_forward_to_last_state : WorkflowSpecApplicationCapture
    {
        private static string toState;

        private Establish context = () =>
        {
            workflowData.Last_State = "test";
        };

        private Because of = () =>
        {
            toState = workflow.GetForwardStateName_Forward_to_last_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            toState.ShouldBeTheSameAs(workflowData.Last_State);
        };
    }
}