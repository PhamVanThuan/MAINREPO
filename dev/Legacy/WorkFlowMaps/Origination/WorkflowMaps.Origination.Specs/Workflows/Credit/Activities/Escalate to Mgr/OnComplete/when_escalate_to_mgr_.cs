using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Escalate_to_Mgr_.OnComplete
{
    [Subject("Activity => Escalate_to_Mgr_ => OnComplete")] // AutoGenerated
    internal class when_escalate_to_mgr_ : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Escalate_to_Mgr_(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}