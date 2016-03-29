using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.FollowupReturn.OnAutoForward
{
    [Subject("State => FollowupReturn => OnAutoForward")]
    internal class when_followupreturn : WorkflowSpecApplicationManagement
    {
        private static string forwardState;

        private Establish context = () =>
        {
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            forwardState = workflow.GetForwardStateName_FollowupReturn(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch(forwardState);
        };
    }
}