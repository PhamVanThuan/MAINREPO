using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Benefits.OnEnter
{
    [Subject("State => Benefits => OnEnter")] // AutoGenerated
    internal class when_benefits : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Benefits(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}