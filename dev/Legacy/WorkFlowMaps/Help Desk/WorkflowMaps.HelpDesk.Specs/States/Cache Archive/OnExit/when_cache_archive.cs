using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.States.Child_Closed.OnExit
{
    [Subject("State => Cache_Archive => OnExit")] // AutoGenerated
    internal class when_cache_archive : WorkflowSpecHelpDesk
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Cache_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}