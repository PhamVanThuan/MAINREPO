using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Clone.OnComplete
{
    [Subject("Activity => Create_Clone => OnComplete")] // AutoGenerated
    internal class when_create_clone : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Create_Clone(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}