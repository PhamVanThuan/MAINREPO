using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Cancel_Debt_Counselling.OnGetStageTransition
{
    [Subject("Activity => Cancel_Debt_Counselling => OnGetStageTransition")] // AutoGenerated
    internal class when_cancel_debt_counselling : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Cancel_Debt_Counselling(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}