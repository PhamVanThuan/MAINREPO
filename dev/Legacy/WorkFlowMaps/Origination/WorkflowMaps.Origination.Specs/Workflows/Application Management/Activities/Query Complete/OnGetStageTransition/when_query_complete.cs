using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Query_Complete.OnGetStageTransition
{
    [Subject("Activity => Query_Complete => OnGetStageTransition")]
    internal class when_query_complete : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Query_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_query_complete = () =>
        {
            result.ShouldBeTheSameAs("Query Complete");
        };
    }
}