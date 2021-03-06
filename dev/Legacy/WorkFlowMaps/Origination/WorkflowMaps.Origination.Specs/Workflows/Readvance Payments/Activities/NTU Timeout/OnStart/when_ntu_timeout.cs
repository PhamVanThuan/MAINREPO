using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.NTU_Timeout.OnStart
{
    [Subject("Activity => NTU_Timeout => OnStart")] // AutoGenerated
    internal class when_ntu_timeout : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_NTU_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}