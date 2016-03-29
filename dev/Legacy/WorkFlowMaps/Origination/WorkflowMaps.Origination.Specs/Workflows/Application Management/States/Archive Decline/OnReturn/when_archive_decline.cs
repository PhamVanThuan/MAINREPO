using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_Decline.OnReturn
{
    [Subject("State => Archive_Decline => OnReturn")] // AutoGenerated
    internal class when_archive_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}