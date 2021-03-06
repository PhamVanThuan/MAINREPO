using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Continue_Litigation.OnStart
{
    [Subject("Activity => Continue_Litigation => OnStart")] // AutoGenerated
    internal class when_continue_litigation : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Continue_Litigation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}