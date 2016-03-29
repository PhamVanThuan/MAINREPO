using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.RetManageApp.OnAutoForward
{
    [Subject("State => RetManageApp => OnAutoForward")]
    internal class when_retmanageapp : WorkflowSpecApplicationManagement
    {
        private static string forwardState;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            forwardState = workflow.GetForwardStateName_RetManageApp(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_manage_application_as_forward_state = () =>
        {
            forwardState.ShouldMatch("Manage Application");
        };
    }
}