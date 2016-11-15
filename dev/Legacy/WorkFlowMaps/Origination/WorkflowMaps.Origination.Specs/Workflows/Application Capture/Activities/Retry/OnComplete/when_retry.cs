using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Retry.OnComplete
{
    [Subject("Activity => Retry => OnComplete")] // AutoGenerated
    internal class when_retry : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Retry(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}