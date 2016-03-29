using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Reactivate__App_Not_Accepted.OnGetStageTransition
{
    [Subject("Activity => Reactivate__App_Not_Accepted => OnGetStageTransition")]
    internal class when_reactivate_app_not_accepted : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reactivate__App_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_reactivate_app_not_accepted = () =>
        {
            result.ShouldMatch("Reactivate  App Not Accepted");
        };
    }
}