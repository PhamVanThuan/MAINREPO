using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Invalid_Application.OnComplete
{
    [Subject("Activity => Invalid_Application => OnComplete")] // AutoGenerated
    internal class when_invalid_application : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Invalid_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}