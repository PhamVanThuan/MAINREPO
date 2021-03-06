using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_in_Order.OnGetActivityMessage
{
    [Subject("Activity => Payment_in_Order => OnGetActivityMessage")] // AutoGenerated
    internal class when_payment_in_order : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Payment_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}