using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.NTU.OnGetActivityMessage
{
    [Subject("Activity => NTU => OnGetActivityMessage")] // AutoGenerated
    internal class when_ntu : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}