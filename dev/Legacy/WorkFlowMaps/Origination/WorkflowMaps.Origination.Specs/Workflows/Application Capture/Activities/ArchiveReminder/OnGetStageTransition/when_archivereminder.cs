using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.ArchiveReminder.OnGetStageTransition
{
    [Subject("Activity => ArchiveReminder => OnGetStageTransition")] // AutoGenerated
    internal class when_archivereminder : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_ArchiveReminder(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}