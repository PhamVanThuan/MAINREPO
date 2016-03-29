using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_Decline.OnGetStageTransition
{
    [Subject("Activity => Reinstate_Decline => OnGetStageTransition")]
    internal class when_reinstate_decline : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reinstate_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_reinstate_decline = () =>
        {
            result.ShouldEqual("Reinstate Decline");
        };
    }
}