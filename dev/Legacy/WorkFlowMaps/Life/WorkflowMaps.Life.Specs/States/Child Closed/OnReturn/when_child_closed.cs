using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Child_Closed.OnReturn
{
    [Subject("State => Child_Closed => OnReturn")] // AutoGenerated
    internal class when_child_closed : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Child_Closed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}