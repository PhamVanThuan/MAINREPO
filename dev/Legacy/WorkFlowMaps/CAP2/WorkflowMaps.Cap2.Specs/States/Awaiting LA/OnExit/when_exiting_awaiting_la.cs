using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Awaiting_LA.OnExit
{
    [Subject("State => Awaiting_LA => OnExit")]
    internal class when_exiting_awaiting_la : WorkflowSpecCap2
    {
        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            workflow.OnExit_Awaiting_LA(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_the_last_state_data_property_to_awaiting_la = () =>
        {
            workflowData.Last_State.ShouldEqual<string>("Awaiting LA");
        };
    }
}