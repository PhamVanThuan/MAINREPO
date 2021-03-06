using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Valuation_Workflow.OnComplete
{
    [Subject("Activity => Valuation_Workflow => OnComplete")] // AutoGenerated
    internal class when_valuation_workflow : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Valuation_Workflow(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}