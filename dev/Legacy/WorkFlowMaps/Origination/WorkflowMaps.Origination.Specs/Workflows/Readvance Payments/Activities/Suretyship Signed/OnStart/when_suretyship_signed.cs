using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Suretyship_Signed.OnStart
{
    [Subject("Activity => Suretyship_Signed => OnStart")] // AutoGenerated
    internal class when_suretyship_signed : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Suretyship_Signed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}