using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Client_Refuse.OnGetStageTransition
{
    [Subject("Activity => Client_Refuse => OnGetStageTransition")]
    internal class when_client_refuse : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Client_Refuse(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_client_refuse = () =>
        {
            result.ShouldMatch("Client Refuse");
        };
    }
}