using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Override_Decision.OnStart
{
    [Subject("Activity => Override_Decision => OnStart")] // AutoGenerated
    internal class when_override_decision : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Override_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}