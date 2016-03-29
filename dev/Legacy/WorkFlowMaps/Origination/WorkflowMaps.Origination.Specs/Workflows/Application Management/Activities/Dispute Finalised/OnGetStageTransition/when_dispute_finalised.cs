using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Dispute_Finalised.OnGetStageTransition
{
    [Subject("Activity => Dispute_Finalised => OnGetStageTransition")]
    internal class when_dispute_finalised : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Dispute_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_dispute_finalised = () =>
        {
            result.ShouldMatch("Dispute Finalised");
        };
    }
}