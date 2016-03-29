using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment.OnComplete
{
    [Subject("Activity => Cash_Payment => OnComplete")]
    internal class when_cash_payment : WorkflowSpecCap2
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cash_Payment(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}