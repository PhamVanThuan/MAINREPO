using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment_Verified.OnComplete
{
    [Subject("Activity => Cash_Payment_Verified => OnComplete")]
    internal class when_cash_payment_verified : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            message = string.Empty;
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cash_Payment_Verified(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}