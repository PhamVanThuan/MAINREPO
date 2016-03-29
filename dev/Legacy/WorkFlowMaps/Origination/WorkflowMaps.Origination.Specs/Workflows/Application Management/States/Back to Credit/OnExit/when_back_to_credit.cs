using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Back_to_Credit.OnExit
{
    [Subject("State => Back_to_Credit => OnExit")] // AutoGenerated
    internal class when_back_to_credit : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Back_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}