using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Override_Decision.OnGetActivityMessage
{
    [Subject("Activity => Override_Decision => OnGetActivityMessage")] // AutoGenerated
    internal class when_override_decision : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Override_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}