using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Change_Policy_Type.OnComplete
{
    [Subject("Activity => Change_Policy_Type => OnComplete")] // AutoGenerated
    internal class when_change_policy_type : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Change_Policy_Type(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}