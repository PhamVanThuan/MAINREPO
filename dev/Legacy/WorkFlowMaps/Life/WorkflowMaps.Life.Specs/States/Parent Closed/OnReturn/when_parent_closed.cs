using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Parent_Closed.OnReturn
{
    [Subject("State => Parent_Closed => OnReturn")] // AutoGenerated
    internal class when_parent_closed : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Parent_Closed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}