using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.FAIS.OnExit
{
    [Subject("State => FAIS => OnExit")] // AutoGenerated
    internal class when_fais : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_FAIS(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}