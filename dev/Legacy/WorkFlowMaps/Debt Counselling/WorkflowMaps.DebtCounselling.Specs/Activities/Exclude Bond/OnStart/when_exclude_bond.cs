using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Exclude_Bond.OnStart
{
    [Subject("Activity => Exclude_Bond => OnStart")] // AutoGenerated
    internal class when_exclude_bond : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Exclude_Bond(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}