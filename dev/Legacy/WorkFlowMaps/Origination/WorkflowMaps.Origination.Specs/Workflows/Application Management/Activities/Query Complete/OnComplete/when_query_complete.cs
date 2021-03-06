using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Query_Complete.OnComplete
{
    [Subject("Activity => Query_Complete => OnComplete")] // AutoGenerated
    internal class when_query_complete : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Query_Complete(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}