using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Return_To_Previous_State.OnEnter
{
    [Subject("State => Return_To_Previous_State => OnEnter")] // AutoGenerated
    internal class when_return_to_previous_state : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Return_To_Previous_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}