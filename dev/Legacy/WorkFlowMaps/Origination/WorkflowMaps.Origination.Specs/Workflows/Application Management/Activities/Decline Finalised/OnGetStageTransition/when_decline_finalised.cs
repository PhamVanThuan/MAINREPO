using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline_Finalised.OnGetStageTransition
{
    [Subject("Activity => Decline_Finalised => OnGetStageTransition")]
    internal class when_decline_finalised : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline_finalised = () =>
        {
            result.ShouldMatch("Decline Finalised");
        };
    }
}