using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.OptOut_Return.OnExit
{
    [Subject("State => OptOut_Return => OnExit")] // AutoGenerated
    internal class when_optout_return : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_OptOut_Return(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}