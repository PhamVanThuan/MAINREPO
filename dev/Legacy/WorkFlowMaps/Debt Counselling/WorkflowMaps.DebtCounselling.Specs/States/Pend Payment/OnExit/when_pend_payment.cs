using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Pend_Payment.OnExit
{
    [Subject("State => Pend_Payment => OnExit")] // AutoGenerated
    internal class when_pend_payment : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Pend_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}