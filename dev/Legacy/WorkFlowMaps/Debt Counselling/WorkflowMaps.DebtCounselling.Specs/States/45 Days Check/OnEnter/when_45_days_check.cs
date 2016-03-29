using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States._45_Days_Check.OnEnter
{
    [Subject("State => 45_Days_Check => OnEnter")] // AutoGenerated
    internal class when_45_days_check : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_45_Days_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}