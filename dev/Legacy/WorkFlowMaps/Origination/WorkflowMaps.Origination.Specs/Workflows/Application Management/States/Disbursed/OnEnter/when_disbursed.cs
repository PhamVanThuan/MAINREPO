using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Disbursed.OnEnter
{
    [Subject("State => Disbursed => OnEnter")] // AutoGenerated
    internal class when_disbursed : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Disbursed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}