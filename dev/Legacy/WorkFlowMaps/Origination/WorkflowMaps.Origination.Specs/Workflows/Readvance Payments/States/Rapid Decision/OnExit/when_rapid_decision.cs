using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Rapid_Decision.OnExit
{
    [Subject("State => Rapid_Decision => OnExit")]
    internal class when_rapid_decision : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Rapid_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state = () =>
        {
            workflowData.PreviousState.ShouldMatch("Rapid Decision");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}