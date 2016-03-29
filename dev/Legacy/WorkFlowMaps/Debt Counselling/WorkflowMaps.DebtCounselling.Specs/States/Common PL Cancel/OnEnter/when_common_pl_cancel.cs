using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Common_PL_Cancel.OnEnter
{
    [Subject("State => Common_PL_Cancel => OnEnter")] // AutoGenerated
    internal class when_common_pl_cancel : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_PL_Cancel(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}