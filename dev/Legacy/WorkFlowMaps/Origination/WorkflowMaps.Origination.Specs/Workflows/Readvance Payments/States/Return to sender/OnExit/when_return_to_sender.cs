using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Return_to_sender.OnExit
{
    [Subject("State => Return_to_sender => OnExit")] // AutoGenerated
    internal class when_return_to_sender : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Return_to_sender(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}