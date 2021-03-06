using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Continue_with_Followup.OnGetActivityMessage
{
    [Subject("Activity => Continue_with_Followup => OnGetActivityMessage")] // AutoGenerated
    internal class when_continue_with_followup : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Continue_with_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}