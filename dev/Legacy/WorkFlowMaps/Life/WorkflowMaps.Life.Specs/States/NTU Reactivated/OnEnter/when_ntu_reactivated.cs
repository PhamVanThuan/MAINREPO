using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.NTU_Reactivated.OnEnter
{
    [Subject("State => NTU_Reactivated => OnEnter")] // AutoGenerated
    internal class when_ntu_reactivated : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_NTU_Reactivated(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}