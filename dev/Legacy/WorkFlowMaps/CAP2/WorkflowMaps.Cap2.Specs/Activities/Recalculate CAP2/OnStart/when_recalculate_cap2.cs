using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Recalculate_CAP2.OnStart
{
    [Subject("Activity => Recalculate_CAP2 => OnStart")] // AutoGenerated
    internal class when_recalculate_cap2 : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Recalculate_CAP2(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}