using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Not_Direct_Consultant.OnComplete
{
    [Subject("Activity => Not_Direct_Consultant => OnComplete")] // AutoGenerated
    internal class when_not_direct_consultant : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Not_Direct_Consultant(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}