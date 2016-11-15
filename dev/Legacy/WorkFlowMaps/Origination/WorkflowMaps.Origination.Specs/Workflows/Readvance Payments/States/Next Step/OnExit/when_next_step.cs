using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Next_Step.OnExit
{
    [Subject("State => Next_Step => OnExit")] // AutoGenerated
    internal class when_next_step : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Next_Step(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}