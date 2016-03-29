using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Disbursement_Incorrect.OnGetStageTransition
{
    [Subject("Activity => Disbursement_Incorrect => OnGetStageTransition")]
    internal class when_disbursement_incorrect : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Disbursement_Incorrect(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_disbursement_incorrect = () =>
        {
            result.ShouldMatch("Disbursement Incorrect");
        };
    }
}