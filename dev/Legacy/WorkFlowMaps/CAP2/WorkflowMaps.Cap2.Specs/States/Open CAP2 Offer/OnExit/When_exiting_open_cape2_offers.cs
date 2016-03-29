using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Open_CAP2_Offer.OnExit
{
    [Subject("State => Open_CAP2_Offer => OnExit")]
    public class When_exiting_open_cape2_offers : WorkflowSpecCap2
    {
        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            workflow.OnExit_Open_CAP2_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_the_last_state_data_property_to_open_cap2_offer = () =>
        {
            workflowData.Last_State.ShouldEqual<string>("Open CAP2 Offer");
        };
    }
}