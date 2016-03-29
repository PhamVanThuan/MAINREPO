using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Return_to_sender.OnAutoForward
{
    [Subject("State => Return to sender => OnAutoForward")]
    internal class when_return_to_sender : WorkflowSpecReadvancePayments
    {
        private static string forwardState;

        private Establish context = () =>
        {
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            forwardState = workflow.GetForwardStateName_Return_to_sender(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch(forwardState);
        };
    }
}