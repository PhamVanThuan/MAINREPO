using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Application_Received.OnGetStageTransition
{
    [Subject("Activity => Application_Received => OnGetStageTransition")]
    internal class when_application_recieved : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Application_Received(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_application_received = () =>
        {
            result.ShouldMatch("Application Received");
        };
    }
}