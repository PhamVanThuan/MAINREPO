using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.No_Court_Date_or_Deposit.OnComplete
{
    [Subject("Activity => No_Court_Date_or_Deposit => OnComplete")] // AutoGenerated
    internal class when_no_court_date_or_deposit : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_No_Court_Date_or_Deposit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}