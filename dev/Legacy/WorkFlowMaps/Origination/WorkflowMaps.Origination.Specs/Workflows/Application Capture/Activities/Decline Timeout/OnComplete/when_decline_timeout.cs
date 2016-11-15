using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Decline_Timeout.OnComplete
{
    [Subject("Activity => Decline_Timeout => OnComplete")] // AutoGenerated
    internal class when_decline_timeout : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Decline_Timeout(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}