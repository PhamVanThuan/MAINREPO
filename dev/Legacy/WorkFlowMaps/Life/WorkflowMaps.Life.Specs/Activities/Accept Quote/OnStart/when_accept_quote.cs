using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_Quote.OnStart
{
    [Subject("Activity => Accept_Quote => OnStart")] // AutoGenerated
    internal class when_accept_quote : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Accept_Quote(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}