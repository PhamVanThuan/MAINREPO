using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Query_on_LOA.OnGetStageTransition
{
    [Subject("Activity => Query_on_LOA => OnGetStageTransition")]
    internal class when_query_on_loa : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Query_on_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_query_on_LOA = () =>
        {
            result.ShouldBeTheSameAs("Query on LOA");
        };
    }
}