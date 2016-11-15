using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.App_Check.OnExit
{
    [Subject("State => App_Check => OnExit")] // AutoGenerated
    internal class when_app_check : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_App_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}