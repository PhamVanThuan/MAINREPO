using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Manager_Archive.OnStart
{
    [Subject("Activity => Manager_Archive => OnStart")] // AutoGenerated
    internal class when_manager_archive : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Manager_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}