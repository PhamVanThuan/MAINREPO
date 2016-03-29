using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Payment_Prepared.OnGetActivityMessage
{
    [Subject("Activity => Payment_Prepared => OnGetActivityMessage")] // AutoGenerated
    internal class when_payment_prepared : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Payment_Prepared(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}