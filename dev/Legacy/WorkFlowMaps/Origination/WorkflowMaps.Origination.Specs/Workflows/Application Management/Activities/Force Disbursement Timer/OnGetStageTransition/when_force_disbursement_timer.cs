using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Force_Disbursement_Timer.OnGetStageTransition
{
    [Subject("Activity => Force_Disbursement_Timer => OnGetStageTransition")]
    internal class when_force_disbursement_timer : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Force_Disbursement_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_force_disbursement_timer = () =>
        {
            result.ShouldMatch("Force Disbursement Timer");
        };
    }
}