using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Continue_another_term.OnGetActivityMessage
{
    [Subject("Activity => Continue_another_term => OnGetActivityMessage")] // AutoGenerated
    internal class when_continue_another_term : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Continue_another_term(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}