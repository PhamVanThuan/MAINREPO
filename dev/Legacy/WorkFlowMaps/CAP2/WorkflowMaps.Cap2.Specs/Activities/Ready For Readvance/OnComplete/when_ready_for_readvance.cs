using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Ready_For_Readvance.OnComplete
{
    [Subject("Activity => Ready_For_Readvance => OnComplete")] // AutoGenerated
    internal class when_ready_for_readvance : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Ready_For_Readvance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}