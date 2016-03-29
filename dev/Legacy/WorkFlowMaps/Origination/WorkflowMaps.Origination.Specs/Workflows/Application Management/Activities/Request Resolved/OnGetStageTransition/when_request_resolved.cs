using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Request_Resolved.OnGetStageTransition
{
    [Subject("Activity => Request_Resolved => OnGetStageTransition")]
    internal class when_request_resolved : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Resolved(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_request_resolved = () =>
        {
            result.ShouldEqual("Request Resolved");
        };
    }
}