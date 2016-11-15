using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Child_Closed.OnReturn
{
    [Subject("State => Cache_Archive => OnReturn")] // AutoGenerated
    internal class when_cache_archive : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Cache_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}