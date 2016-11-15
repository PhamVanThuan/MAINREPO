using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Archive_Lead.OnStart
{
    [Subject("Activity => Archive_Lead => OnStart")] // AutoGenerated
    internal class when_archive_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Archive_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}