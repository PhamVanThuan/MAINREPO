using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_FL.OnReturn
{
    [Subject("State => Archive_FL => OnReturn")] // AutoGenerated
    internal class when_archive_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_FL(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}