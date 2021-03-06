using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Back_to_Credit.OnEnter
{
    [Subject("State => Back_to_Credit => OnEnter")] // AutoGenerated
    internal class when_back_to_credit : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Back_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}