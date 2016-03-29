using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Forward_to_last_State.OnAutoForward
{
    [Subject("State => Forward_to_last_State => OnAutoForward")]
    internal class when_auto_forward : WorkflowSpecCap2
    {
        private static string lastState;
        private static string autoFwdState;

        private Establish context = () =>
        {
            lastState = "Open CAP2 Offer";
            workflowData.Last_State = lastState;
        };

        private Because of = () =>
        {
            autoFwdState = workflow.GetForwardStateName_Forward_to_last_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_last_state_data_property = () =>
        {
            autoFwdState.ShouldEqual(workflowData.Last_State);
        };
    }
}