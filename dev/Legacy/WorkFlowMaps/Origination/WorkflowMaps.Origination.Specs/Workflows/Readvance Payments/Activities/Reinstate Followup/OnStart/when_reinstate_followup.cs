using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Reinstate_Followup.OnStart
{
    [Subject("Activity => Reinstate_Followup => OnStart")] // AutoGenerated
    internal class when_reinstate_followup : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reinstate_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}