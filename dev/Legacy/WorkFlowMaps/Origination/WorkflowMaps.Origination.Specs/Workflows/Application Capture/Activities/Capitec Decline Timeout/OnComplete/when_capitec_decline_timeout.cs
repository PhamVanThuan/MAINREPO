using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Capitec_Decline_Timeout.OnComplete
{
    [Subject("Activity => Capitec_Decline_Timeout => OnComplete")] // AutoGenerated
    internal class when_capitec_decline_timeout : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Capitec_Decline_Timeout(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}