using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Exclusions.OnExit
{
    [Subject("State => Exclusions => OnExit")] // AutoGenerated
    internal class when_exclusions : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Exclusions(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}