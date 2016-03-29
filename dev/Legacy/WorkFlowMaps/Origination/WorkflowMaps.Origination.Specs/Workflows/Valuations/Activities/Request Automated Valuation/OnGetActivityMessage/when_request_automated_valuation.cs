using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Request_Automated_Valuation.OnGetActivityMessage
{
    [Subject("Activity => Request_Automated_Valuation => OnGetActivityMessage")] // AutoGenerated
    internal class when_request_automated_valuation : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Request_Automated_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}