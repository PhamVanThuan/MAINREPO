using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Awaiting_Forms.OnExit
{
    [Subject("State => Awaiting_Forms => OnExit")]
    internal class when_exiting_awaiting_forms : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
            {
                result = false;
            };

        private Because of = () =>
            {
                result = workflow.OnExit_Awaiting_Forms(instanceData, workflowData, paramsData, messages);
            };

        private It should_set_the_last_state_data_property_to_awaiting_forms = () =>
            {
                workflowData.Last_State.ShouldEqual<string>("Awaiting Forms");
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}