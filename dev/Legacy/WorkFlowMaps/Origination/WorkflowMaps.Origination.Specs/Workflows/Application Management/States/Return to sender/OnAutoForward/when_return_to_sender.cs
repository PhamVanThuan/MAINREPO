using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Return_to_sender.OnAutoForward
{
    [Subject("State => Return_to_sender => OnAutoForward")]
    internal class when_return_to_sender : WorkflowSpecApplicationManagement
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