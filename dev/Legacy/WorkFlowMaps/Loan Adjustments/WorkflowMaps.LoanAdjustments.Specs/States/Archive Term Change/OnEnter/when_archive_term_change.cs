using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Archive_Term_Change.OnEnter
{
    [Subject("State => Archive_Term_Change => OnEnter")] // AutoGenerated
    internal class when_archive_term_change : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Term_Change(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}