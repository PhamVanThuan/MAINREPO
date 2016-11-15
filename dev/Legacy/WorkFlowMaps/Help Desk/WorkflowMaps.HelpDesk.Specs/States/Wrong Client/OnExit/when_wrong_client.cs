using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.States.Wrong_Client.OnExit
{
    [Subject("State => Wrong_Client => OnExit")] // AutoGenerated
    internal class when_wrong_client : WorkflowSpecHelpDesk
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Wrong_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}