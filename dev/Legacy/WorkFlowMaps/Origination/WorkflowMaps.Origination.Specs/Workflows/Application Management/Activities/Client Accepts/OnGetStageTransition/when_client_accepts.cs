using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Client_Accepts.OnGetStageTransition
{
    [Subject("Activity => Client_Accepts => OnGetStageTransition")]
    internal class when_client_accepts : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Client_Accepts(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_client_accepts = () =>
        {
            result.ShouldMatch("Client Accepts");
        };
    }
}