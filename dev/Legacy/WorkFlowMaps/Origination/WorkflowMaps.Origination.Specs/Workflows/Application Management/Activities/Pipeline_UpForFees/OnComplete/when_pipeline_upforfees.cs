using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Pipeline_UpForFees.OnComplete
{
    [Subject("Activity => Pipeline_UpForFees => OnComplete")] // AutoGenerated
    internal class when_pipeline_upforfees : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Pipeline_UpForFees(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}