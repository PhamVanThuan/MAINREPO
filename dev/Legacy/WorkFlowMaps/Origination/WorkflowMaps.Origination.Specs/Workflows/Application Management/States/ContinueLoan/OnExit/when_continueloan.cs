using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.ContinueLoan.OnExit
{
    [Subject("State => ContinueLoan => OnExit")] // AutoGenerated
    internal class when_continueloan : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_ContinueLoan(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}