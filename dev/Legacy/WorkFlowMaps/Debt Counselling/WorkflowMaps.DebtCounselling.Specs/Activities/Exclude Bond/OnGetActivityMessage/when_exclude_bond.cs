using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Exclude_Bond.OnGetActivityMessage
{
    [Subject("Activity => Exclude_Bond => OnGetActivityMessage")] // AutoGenerated
    internal class when_exclude_bond : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Exclude_Bond(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}