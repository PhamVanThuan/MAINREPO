using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline_Final.OnGetStageTransition
{
    [Subject("Activity => Decline_Final => OnGetStageTransition")]
    internal class when_decline_final : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_Final(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline_final = () =>
        {
            result.ShouldMatch("Decline Final");
        };
    }
}