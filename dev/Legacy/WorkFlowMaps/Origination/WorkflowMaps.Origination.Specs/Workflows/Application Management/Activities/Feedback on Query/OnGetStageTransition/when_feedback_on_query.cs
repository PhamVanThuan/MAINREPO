using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Feedback_on_Query.OnGetStageTransition
{
    [Subject("Activity => Feedback_on_Query => OnGetStageTransition")]
    internal class when_feedback_on_query : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Feedback_on_Query(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_feedback_on_query_received_from_branch = () =>
        {
            result.ShouldMatch("Feedback on Query received from Branch");
        };
    }
}