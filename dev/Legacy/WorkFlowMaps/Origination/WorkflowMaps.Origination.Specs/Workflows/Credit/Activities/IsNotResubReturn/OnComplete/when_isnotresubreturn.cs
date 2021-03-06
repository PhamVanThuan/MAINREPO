using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.IsNotResubReturn.OnComplete
{
    [Subject("Activity => IsNotResubReturn => OnComplete")] // AutoGenerated
    internal class when_isnotresubreturn : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_IsNotResubReturn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}