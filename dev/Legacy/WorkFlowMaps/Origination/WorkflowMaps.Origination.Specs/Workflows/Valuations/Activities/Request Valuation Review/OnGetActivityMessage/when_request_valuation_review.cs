using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Request_Valuation_Review.OnGetActivityMessage
{
    [Subject("Activity => Request_Valuation_Review => OnGetActivityMessage")] // AutoGenerated
    internal class when_request_valuation_review : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Request_Valuation_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}