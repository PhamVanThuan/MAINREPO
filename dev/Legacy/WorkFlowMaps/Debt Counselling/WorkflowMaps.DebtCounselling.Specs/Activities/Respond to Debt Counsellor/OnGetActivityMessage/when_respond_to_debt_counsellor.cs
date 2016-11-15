using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Respond_to_Debt_Counsellor.OnGetActivityMessage
{
    [Subject("Activity => Respond_to_Debt_Counsellor => OnGetActivityMessage")] // AutoGenerated
    internal class when_respond_to_debt_counsellor : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Respond_to_Debt_Counsellor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}