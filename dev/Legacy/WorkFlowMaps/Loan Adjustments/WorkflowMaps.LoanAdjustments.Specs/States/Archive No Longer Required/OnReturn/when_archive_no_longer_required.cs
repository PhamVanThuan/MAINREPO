using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Archive_No_Longer_Required.OnReturn
{
    [Subject("State => Archive_No_Longer_Required => OnReturn")] // AutoGenerated
    internal class when_archive_no_longer_required : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_No_Longer_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}