using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Archive_Reminder.OnExit
{
    [Subject("State => Archive_Reminder => OnExit")] // AutoGenerated
    internal class when_archive_reminder : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Reminder(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}