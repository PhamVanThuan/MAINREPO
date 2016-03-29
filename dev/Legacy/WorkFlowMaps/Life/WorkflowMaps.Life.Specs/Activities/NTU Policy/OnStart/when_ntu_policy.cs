using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.NTU_Policy.OnStart
{
    [Subject("Activity => NTU_Policy => OnStart")] // AutoGenerated
    internal class when_ntu_policy : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_NTU_Policy(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}