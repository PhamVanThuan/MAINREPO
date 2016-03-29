using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.GotoCredit.OnStart
{
    [Subject("Activity => GotoCredit => OnStart")] // AutoGenerated
    internal class when_gotocredit : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_GotoCredit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}