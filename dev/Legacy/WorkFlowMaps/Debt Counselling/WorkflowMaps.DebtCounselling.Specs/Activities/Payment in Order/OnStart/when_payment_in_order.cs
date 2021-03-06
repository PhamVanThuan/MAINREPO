using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_in_Order.OnStart
{
    [Subject("Activity => Payment_in_Order => OnStart")] // AutoGenerated
    internal class when_payment_in_order : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Payment_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}