using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline.OnGetStageTransition
{
    [Subject("Activity => Decline => OnGetStageTransition")]
    internal class when_decline : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline = () =>
        {
            result.ShouldMatch("Decline");
        };
    }
}