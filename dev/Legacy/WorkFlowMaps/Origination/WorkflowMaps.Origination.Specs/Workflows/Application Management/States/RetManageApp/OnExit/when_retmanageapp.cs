using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.RetManageApp.OnExit
{
    [Subject("State => RetManageApp => OnExit")] // AutoGenerated
    internal class when_retmanageapp : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_RetManageApp(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}