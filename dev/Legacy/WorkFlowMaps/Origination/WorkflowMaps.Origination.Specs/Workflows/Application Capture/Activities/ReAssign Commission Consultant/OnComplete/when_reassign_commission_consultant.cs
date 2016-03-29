using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.ReAssign_Commission_Consultant.OnComplete
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnComplete")] // AutoGenerated
    internal class when_reassign_commission_consultant : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}