using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_Received.OnGetActivityMessage
{
    [Subject("Activity => Payment_Received => OnGetActivityMessage")] // AutoGenerated
    internal class when_payment_received : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Payment_Received(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}