using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities._12_Hour_Override.OnGetActivityMessage
{
    [Subject("Activity => 12_Hour_Override => OnGetActivityMessage")] // AutoGenerated
    internal class when_12_hour_override : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_12_Hour_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}