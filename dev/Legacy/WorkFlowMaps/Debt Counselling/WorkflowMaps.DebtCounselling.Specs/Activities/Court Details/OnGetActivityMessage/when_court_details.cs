using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Details.OnGetActivityMessage
{
    [Subject("Activity => Court_Details => OnGetActivityMessage")] // AutoGenerated
    internal class when_court_details : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Court_Details(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}