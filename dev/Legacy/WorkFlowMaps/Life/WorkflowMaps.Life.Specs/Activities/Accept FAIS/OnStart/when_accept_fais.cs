using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_FAIS.OnStart
{
    [Subject("Activity => Accept_FAIS => OnStart")] // AutoGenerated
    internal class when_accept_fais : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Accept_FAIS(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}