using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Further_Info.OnComplete
{
    [Subject("Activity => Further_Info => OnComplete")] // AutoGenerated
    internal class when_further_info : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Further_Info(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}