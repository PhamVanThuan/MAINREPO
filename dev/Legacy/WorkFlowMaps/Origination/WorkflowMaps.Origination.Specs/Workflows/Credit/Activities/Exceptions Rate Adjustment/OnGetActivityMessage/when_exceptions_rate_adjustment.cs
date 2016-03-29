using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Exceptions_Rate_Adjustment.OnGetActivityMessage
{
    [Subject("Activity => Exceptions_Rate_Adjustment => OnGetActivityMessage")] // AutoGenerated
    internal class when_exceptions_rate_adjustment : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Exceptions_Rate_Adjustment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}