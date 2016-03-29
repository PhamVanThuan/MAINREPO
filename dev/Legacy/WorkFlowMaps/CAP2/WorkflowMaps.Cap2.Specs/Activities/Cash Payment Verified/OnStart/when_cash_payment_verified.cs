using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment_Verified.OnStart
{
    [Subject("Activity => Cash_Payment_Verified( => OnStart")]
    internal class when_cash_payment_verified : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Cash_Payment_Verified(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}