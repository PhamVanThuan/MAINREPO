using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Info_Request_Complete.OnGetStageTransition
{
    [Subject("Activity => Info_Request_Complete => OnGetStageTransition")]
    internal class when_info_request_complete : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Info_Request_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_info_request_complete = () =>
        {
            result.ShouldBeTheSameAs("Info Request Complete");
        };
    }
}