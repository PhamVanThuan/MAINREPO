using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Reinstate_Followup.OnGetActivityMessage
{
    [Subject("Activity => Reinstate_Followup => OnGetActivityMessage")] // AutoGenerated
    internal class when_reinstate_followup : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reinstate_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}