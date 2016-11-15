using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Application_Hold.OnExit
{
    [Subject("State => Application_Hold => OnExit")] // AutoGenerated
    internal class when_application_hold : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Application_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}