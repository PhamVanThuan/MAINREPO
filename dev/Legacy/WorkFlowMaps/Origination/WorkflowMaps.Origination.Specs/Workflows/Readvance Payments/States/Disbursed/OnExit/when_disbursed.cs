using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Disbursed.OnExit
{
    [Subject("State => Disbursed => OnExit")]
    internal class when_disbursed : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Disbursed(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_data = () =>
        {
            workflowData.PreviousState.ShouldMatch("Disbursed");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}