using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Decline_by_Credit.OnComplete
{
    [Subject("Activity => Decline_by_Credit => OnComplete")] // AutoGenerated
    internal class when_decline_by_credit : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Decline_by_Credit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}