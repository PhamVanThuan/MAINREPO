using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Decline_Final.OnStart
{
    [Subject("Activity => Decline_Final => OnStart")] // AutoGenerated
    internal class when_decline_final : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline_Final(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}