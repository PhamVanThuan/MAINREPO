using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Attorney_to_Oppose.OnGetActivityMessage
{
    [Subject("Activity => Attorney_to_Oppose => OnGetActivityMessage")] // AutoGenerated
    internal class when_attorney_to_oppose : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Attorney_to_Oppose(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}