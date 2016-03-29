using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Manager_Archive.OnComplete
{
    [Subject("Activity => Manager_Archive => OnComplete")] // AutoGenerated
    internal class when_manager_archive : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Manager_Archive(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}