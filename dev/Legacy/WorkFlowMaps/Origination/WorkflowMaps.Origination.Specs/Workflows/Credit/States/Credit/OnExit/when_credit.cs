using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Credit.OnExit
{
    [Subject("State => Credit => OnExit")]
    internal class when_credit : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property_to_empty_string = () =>
        {
            workflowData.PreviousState.ShouldBeEmpty();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}