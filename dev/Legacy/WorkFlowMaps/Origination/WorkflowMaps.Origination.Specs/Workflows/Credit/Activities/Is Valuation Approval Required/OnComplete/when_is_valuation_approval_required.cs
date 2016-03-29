using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Is_Valuation_Approval_Required.OnComplete
{
    [Subject("Activity => Is_Valuation_Approval_Required => OnComplete")] // AutoGenerated
    internal class when_is_valuation_approval_required : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Is_Valuation_Approval_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}