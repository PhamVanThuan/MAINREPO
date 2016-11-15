using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Return_to_Processor.OnStart
{
    [Subject("Activity => Return_to_Processor => OnStart")] // AutoGenerated
    internal class when_return_to_processor : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Return_to_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}