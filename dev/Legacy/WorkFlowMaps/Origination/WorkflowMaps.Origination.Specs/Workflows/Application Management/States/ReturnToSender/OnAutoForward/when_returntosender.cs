using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.ReturnToSender.OnAutoForward
{
    [Subject("State => ReturnToSender => OnAutoForward")]
    internal class when_returntosender : WorkflowSpecApplicationManagement
    {
        private static string forwardState;

        private Establish context = () =>
        {
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            forwardState = workflow.GetForwardStateName_ReturnToSender(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch(forwardState);
        };
    }
}