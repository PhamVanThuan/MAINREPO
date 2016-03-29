using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.OnSplitReturn.OnComplete
{
    [Subject("Activity => OnSplitReturn => OnComplete")] // AutoGenerated
    internal class when_onsplitreturn : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_OnSplitReturn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}