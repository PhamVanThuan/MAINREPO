using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.EXT_Under_Cancellation.OnStart
{
    [Subject("Activity => EXT_Under_Cancellation => OnStart")] // AutoGenerated
    internal class when_ext_under_cancellation : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXT_Under_Cancellation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}