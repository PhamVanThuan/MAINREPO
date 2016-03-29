using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Policy_Override.OnGetActivityMessage
{
    [Subject("Activity => Request_Policy_Override => OnGetActivityMessage")]
    internal class when_request_policy_override : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Request_Policy_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}