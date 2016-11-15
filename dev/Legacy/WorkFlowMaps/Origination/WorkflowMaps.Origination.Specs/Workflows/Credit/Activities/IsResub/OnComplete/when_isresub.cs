using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.IsResub.OnComplete
{
    [Subject("Activity => IsResub => OnComplete")] // AutoGenerated
    internal class when_isresub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_IsResub(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}