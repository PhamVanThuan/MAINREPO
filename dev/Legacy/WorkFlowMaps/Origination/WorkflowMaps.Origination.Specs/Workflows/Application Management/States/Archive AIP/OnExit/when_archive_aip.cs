using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_AIP.OnExit
{
    [Subject("State => Archive_AIP => OnExit")] // AutoGenerated
    internal class when_archive_aip : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_AIP(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}