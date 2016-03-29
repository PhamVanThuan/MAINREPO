using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Awaiting_LA.OnEnter
{
    [Subject("State => Awaiting_LA => OnEnter")]
    internal class when_entering_awaiting_la : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Awaiting_LA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}