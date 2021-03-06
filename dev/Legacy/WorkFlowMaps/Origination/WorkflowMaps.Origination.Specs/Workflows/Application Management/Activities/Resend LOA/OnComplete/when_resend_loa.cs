using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Resend_LOA.OnComplete
{
    [Subject("Activity => Resend_LOA => OnComplete")] // AutoGenerated
    internal class when_resend_loa : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Resend_LOA(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}