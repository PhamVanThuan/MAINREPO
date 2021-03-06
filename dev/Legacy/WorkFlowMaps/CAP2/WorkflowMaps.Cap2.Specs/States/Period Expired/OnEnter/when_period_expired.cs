using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Period_Expired.OnEnter
{
    [Subject("State => Period_Expired => OnEnter")] // AutoGenerated
    internal class when_period_expired : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Period_Expired(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}