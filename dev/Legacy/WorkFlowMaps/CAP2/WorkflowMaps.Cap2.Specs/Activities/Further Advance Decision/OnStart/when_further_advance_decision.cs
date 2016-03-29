using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Further_Advance_Decision.OnStart
{
    [Subject("Activity => Further_Advance_Decision => OnStart")]
    internal class when_further_advance_decision : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Advance_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}