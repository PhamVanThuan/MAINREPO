using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Check_No_Date_No_Pay.OnEnter
{
    [Subject("State => Check_No_Date_No_Pay => OnEnter")] // AutoGenerated
    internal class when_check_no_date_no_pay : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Check_No_Date_No_Pay(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}