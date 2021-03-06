using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Application_Adjustment.OnExit
{
    [Subject("State => Application_Adjustment => OnExit")] // AutoGenerated
    internal class when_application_adjustment : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Application_Adjustment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}