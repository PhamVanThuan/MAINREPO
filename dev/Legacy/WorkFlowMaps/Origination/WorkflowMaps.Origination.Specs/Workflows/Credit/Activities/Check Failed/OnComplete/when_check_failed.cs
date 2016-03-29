using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Check_Failed.OnComplete
{
    [Subject("Activity => Check_Failed => OnComplete")] // AutoGenerated
    internal class when_check_failed : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Check_Failed(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}