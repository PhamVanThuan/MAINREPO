using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Awaiting_Schedule.OnExit
{
    [Subject("State => Awaiting_Schedule => OnExit")]
    internal class when_awaiting_schedule : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Awaiting_Schedule(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_data = () =>
        {
            workflowData.PreviousState.ShouldMatch("Awaiting Schedule");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}