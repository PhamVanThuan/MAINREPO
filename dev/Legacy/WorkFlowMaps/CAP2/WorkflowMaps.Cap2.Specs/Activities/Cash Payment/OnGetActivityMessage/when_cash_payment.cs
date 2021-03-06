using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment.OnGetActivityMessage
{
    [Subject("Activity => Cash_Payment => OnGetActivityMessage")] // AutoGenerated
    internal class when_cash_payment : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Cash_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}