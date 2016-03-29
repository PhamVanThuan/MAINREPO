using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Setup_Payment.OnExit
{
    [Subject("State => Setup_Payment => OnExit")]
    internal class when_setup_payment : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Setup_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state = () =>
        {
            workflowData.PreviousState.ShouldMatch("Setup Payment");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}