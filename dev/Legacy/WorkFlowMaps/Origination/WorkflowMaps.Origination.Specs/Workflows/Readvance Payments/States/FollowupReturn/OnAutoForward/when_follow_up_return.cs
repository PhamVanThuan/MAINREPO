using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.FollowupReturn.OnAutoForward
{
    [Subject("State => FollowupReturn => OnAutoForward")]
    internal class when_follow_up_return : WorkflowSpecReadvancePayments
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