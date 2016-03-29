using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Common_Return_to_Processor.OnExit
{
    [Subject("State => Common_Return_to_Processor => OnExit")] // AutoGenerated
    internal class when_common_return_to_processor : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Return_to_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}