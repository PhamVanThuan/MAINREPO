using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Further_Info.OnGetStageTransition
{
    [Subject("Activity => Request_Further_Info => OnGetStageTransition")]
    internal class when_request_further_info : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Further_Info(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_request_further_info = () =>
        {
            result.ShouldBeTheSameAs("Request Further Info");
        };
    }
}