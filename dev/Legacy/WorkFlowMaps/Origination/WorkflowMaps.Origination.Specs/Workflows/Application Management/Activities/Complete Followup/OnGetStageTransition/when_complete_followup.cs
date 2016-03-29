using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Complete_Followup.OnGetStageTransition
{
    [Subject("Activity => Complete_Followup => OnGetStageTransition")]
    internal class when_complete_followup : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Complete_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_complete_followup = () =>
        {
            result.ShouldMatch("Complete Followup");
        };
    }
}