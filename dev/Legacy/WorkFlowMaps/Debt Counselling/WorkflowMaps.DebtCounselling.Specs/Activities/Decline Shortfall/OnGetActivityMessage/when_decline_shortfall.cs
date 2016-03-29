using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Decline_Shortfall.OnGetActivityMessage
{
    [Subject("Activity => Decline_Shortfall => OnGetActivityMessage")] // AutoGenerated
    internal class when_decline_shortfall : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Decline_Shortfall(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}