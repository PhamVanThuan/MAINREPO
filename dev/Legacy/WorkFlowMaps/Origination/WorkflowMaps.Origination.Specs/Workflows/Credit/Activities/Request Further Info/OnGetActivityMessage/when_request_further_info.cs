using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Request_Further_Info.OnGetActivityMessage
{
    [Subject("Activity => Request_Further_Info => OnGetActivityMessage")] // AutoGenerated
    internal class when_request_further_info : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Request_Further_Info(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}