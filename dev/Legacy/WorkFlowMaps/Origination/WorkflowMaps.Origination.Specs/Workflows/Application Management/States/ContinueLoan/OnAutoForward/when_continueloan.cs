using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.ContinueLoan.OnAutoForward
{
    [Subject("States => ContinueLoan => OnAutoForward")]
    internal class when_continueloan : WorkflowSpecApplicationManagement
    {
        private static string forwardState;

        private Establish context = () =>
        {
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            forwardState = workflow.GetForwardStateName_ContinueLoan(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch(forwardState);
        };
    }
}