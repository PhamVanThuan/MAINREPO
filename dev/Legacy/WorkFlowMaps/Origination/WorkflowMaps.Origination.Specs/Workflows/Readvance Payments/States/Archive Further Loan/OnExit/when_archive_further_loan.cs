using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Archive_Further_Loan.OnExit
{
    [Subject("State => Archive_Further_Loan => OnExit")] // AutoGenerated
    internal class when_archive_further_loan : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Further_Loan(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}