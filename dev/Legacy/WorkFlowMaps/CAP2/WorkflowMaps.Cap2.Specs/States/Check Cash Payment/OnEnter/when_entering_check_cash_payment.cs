using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Check_Cash_Payment.OnEnter
{
    [Subject("State => Check_Cash_Payment => OnEnter")]
    internal class when_entering_check_cash_payment : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Check_Cash_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}