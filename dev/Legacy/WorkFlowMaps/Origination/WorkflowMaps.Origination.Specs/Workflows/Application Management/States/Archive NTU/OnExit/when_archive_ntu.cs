using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_NTU.OnExit
{
    [Subject("State => Archive_NTU => OnExit")] // AutoGenerated
    internal class when_archive_ntu : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}