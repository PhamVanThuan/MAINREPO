using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Policy_Override.OnExit
{
    [Subject("State => Policy_Override => OnExit")]
    internal class when_policy_override : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Policy_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_policy_override_property_to_true = () =>
        {
            workflowData.PolicyOverride.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}