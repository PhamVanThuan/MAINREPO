using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Archive_Decline.OnExit
{
    [Subject("State => Archive_Decline => OnExit")] // AutoGenerated
    internal class when_archive_decline : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}