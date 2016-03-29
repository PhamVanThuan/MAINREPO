using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Send_Schedule.OnExit
{
    [Subject("State => Send_Schedule => OnExit")]
    internal class when_send_schedule : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Send_Schedule(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state = () =>
        {
            workflowData.PreviousState.ShouldMatch("Send Schedule");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}