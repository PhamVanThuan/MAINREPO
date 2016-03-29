using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Estate_Agent_Lead_Assign.OnExit
{
    [Subject("State => Estate_Agent_Lead_Assign => OnExit")]
    internal class when_estate_agent_lead_assign : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Estate_Agent_Lead_Assign(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Estate Agent Lead Assign");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}