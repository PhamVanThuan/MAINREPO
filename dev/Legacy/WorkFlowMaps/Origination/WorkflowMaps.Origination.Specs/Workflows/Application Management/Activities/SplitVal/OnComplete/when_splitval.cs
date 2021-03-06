using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.SplitVal.OnComplete
{
    [Subject("Activity => SplitVal => OnComplete")] // AutoGenerated
    internal class when_splitval : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_SplitVal(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}