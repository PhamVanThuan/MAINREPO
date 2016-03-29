using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Return_to_Processor.OnComplete
{
    [Subject("Activity => Return_to_Processor => OnComplete")] // AutoGenerated
    internal class when_return_to_processor : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Return_to_Processor(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}