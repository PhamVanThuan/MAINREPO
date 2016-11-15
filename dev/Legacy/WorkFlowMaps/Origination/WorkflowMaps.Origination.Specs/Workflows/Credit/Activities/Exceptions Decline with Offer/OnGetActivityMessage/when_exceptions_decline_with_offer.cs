using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Exceptions_Decline_with_Offer.OnGetActivityMessage
{
    [Subject("Activity => Exceptions_Decline_with_Offer => OnGetActivityMessage")] // AutoGenerated
    internal class when_exceptions_decline_with_offer : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Exceptions_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}