using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Further_Advance_Decision.OnComplete
{
    [Subject("Activity => Further_Advance_Decision => OnComplete")]
    internal class when_further_advance_decision : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            message = string.Empty;
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Further_Advance_Decision(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}