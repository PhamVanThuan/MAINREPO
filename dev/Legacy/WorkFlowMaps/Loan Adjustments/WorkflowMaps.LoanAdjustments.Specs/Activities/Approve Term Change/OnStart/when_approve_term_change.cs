using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Approve_Term_Change.OnStart
{
    [Subject("Activity => Approve_Term_Change => OnStart")] // AutoGenerated
    internal class when_approve_term_change : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Approve_Term_Change(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}