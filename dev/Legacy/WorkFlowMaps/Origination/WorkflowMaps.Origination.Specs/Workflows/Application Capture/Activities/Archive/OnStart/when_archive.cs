using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Archive.OnStart
{
    [Subject("Activity => Archive => OnStart")] // AutoGenerated
    internal class when_archive : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}