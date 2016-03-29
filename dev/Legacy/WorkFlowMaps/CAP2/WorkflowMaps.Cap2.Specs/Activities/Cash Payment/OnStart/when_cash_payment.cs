using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment.OnStart
{
    [Subject("Activity => Cash_Payment => OnStart")]
    internal class when_cash_payment : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Cash_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}