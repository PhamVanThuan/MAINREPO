using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Send_Loan_Agreements.OnExit
{
    [Subject("State => Send_Loan_Agreements => OnExit")] // AutoGenerated
    internal class when_send_loan_agreements : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Send_Loan_Agreements(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}