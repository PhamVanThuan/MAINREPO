using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Forward_to_last_State.OnAutoForward
{
    [Subject("State => Forward_to_last_State => OnAutoForward")]
    internal class when_auto_forwarding : WorkflowSpecLife
    {
        private static string state;
        private static string fwdState;

        private Establish context = () =>
        {
            state = "WorkflowState";
            workflowData.LastState = state;
        };

        private Because of = () =>
        {
            fwdState = workflow.GetForwardStateName_Forward_to_last_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_last_state_data_property = () =>
        {
            fwdState.ShouldBeTheSameAs(workflowData.LastState);
        };
    }
}