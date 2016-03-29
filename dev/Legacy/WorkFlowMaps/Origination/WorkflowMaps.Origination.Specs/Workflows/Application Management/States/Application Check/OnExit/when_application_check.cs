using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Application_Check.OnExit
{
    [Subject("State => Application_Check => OnExit")]
    internal class when_application_check : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Application_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Application Check");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}