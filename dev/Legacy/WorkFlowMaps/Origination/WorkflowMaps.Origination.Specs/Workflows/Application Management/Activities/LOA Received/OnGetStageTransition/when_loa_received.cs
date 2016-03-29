using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.LOA_Received.OnGetStageTransition
{
    [Subject("Activity => LOA_Received => OnGetStageTransition")]
    internal class when_loa_received : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_LOA_Received(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_loa_received = () =>
        {
            result.ShouldBeTheSameAs("LOA Received");
        };
    }
}