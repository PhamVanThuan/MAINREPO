using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Ext_Complete.OnComplete
{
    [Subject("Activity => Ext_Complete => OnComplete")] // AutoGenerated
    internal class when_ext_complete : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Ext_Complete(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}