using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Ready_to_Send.OnEnter
{
    [Subject("State => Ready_to_Send => OnEnter")] // AutoGenerated
    internal class when_ready_to_send : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Ready_to_Send(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}