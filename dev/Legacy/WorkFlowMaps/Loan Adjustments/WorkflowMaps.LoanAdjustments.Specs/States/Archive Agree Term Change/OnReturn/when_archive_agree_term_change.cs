using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Archive_Agree_Term_Change.OnReturn
{
    [Subject("State => Archive_Agree_Term_Change => OnReturn")] // AutoGenerated
    internal class when_archive_agree_term_change : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Agree_Term_Change(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}