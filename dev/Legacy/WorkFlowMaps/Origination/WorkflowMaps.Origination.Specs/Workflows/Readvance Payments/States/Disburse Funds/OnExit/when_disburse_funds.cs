using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Disburse_Funds.OnExit
{
    [Subject("State => Disburse_Funds => OnExit")]
    internal class when_disburse_funds : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Disburse_Funds(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_data = () =>
        {
            workflowData.PreviousState.ShouldMatch("Disburse Funds");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}