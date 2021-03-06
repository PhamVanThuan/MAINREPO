using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Valuation_Approved.OnGetActivityMessage
{
    [Subject("Activity => Valuation_Approved => OnGetActivityMessage")] // AutoGenerated
    internal class when_valuation_approved : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Valuation_Approved(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}