using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Archive_Debt_Counselling.OnEnter
{
    [Subject("State => Archive_Debt_Counselling => OnEnter")] // AutoGenerated
    internal class when_archive_debt_counselling : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Debt_Counselling(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}