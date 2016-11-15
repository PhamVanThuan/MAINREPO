using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Common_No_Longer_Required.OnEnter
{
    [Subject("State => Common_No_Longer_Required => OnEnter")] // AutoGenerated
    internal class when_common_no_longer_required : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_No_Longer_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}