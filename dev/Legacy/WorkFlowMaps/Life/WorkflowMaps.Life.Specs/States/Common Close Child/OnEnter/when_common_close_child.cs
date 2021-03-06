using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Common_Close_Child.OnEnter
{
    [Subject("State => Common_Close_Child => OnEnter")] // AutoGenerated
    internal class when_common_close_child : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Close_Child(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}