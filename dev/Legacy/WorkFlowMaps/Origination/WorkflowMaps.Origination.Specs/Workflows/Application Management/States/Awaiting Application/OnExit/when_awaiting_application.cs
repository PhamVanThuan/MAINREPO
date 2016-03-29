using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Awaiting_Application.OnExit
{
    [Subject("States => Awaiting_Application => OnExit")]
    internal class when_awaiting_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Awaiting_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Awaiting Application");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}