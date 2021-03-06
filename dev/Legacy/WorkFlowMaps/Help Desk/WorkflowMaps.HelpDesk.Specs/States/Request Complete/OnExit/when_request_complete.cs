using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.States.Request_Complete.OnExit
{
    [Subject("State => Request_Complete => OnExit")] // AutoGenerated
    internal class when_request_complete : WorkflowSpecHelpDesk
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Request_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}