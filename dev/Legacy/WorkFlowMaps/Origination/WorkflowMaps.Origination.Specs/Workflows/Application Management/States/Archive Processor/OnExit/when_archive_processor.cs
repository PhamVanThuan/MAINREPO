using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_Processor.OnExit
{
    [Subject("State => Archive_Processor => OnExit")] // AutoGenerated
    internal class when_archive_processor : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}