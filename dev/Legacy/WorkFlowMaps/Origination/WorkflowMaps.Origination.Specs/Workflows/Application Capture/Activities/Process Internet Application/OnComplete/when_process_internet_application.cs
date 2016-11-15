using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Process_Internet_Application.OnComplete
{
    [Subject("Activity => Process_Internet_Application => OnComplete")] // AutoGenerated
    internal class when_process_internet_application : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Process_Internet_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}