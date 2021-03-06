using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Common_Return_to_Processor.OnEnter
{
    [Subject("State => Common_Return_to_Processor => OnEnter")] // AutoGenerated
    internal class when_common_return_to_processor : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Return_to_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}