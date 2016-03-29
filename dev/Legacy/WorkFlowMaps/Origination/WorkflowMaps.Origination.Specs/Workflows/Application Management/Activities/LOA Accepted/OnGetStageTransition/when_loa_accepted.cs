using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.LOA_Accepted.OnGetStageTransition
{
    [Subject("Activity => LOA_Accepted => OnGetStageTransition")]
    internal class when_loa_accepted : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_LOA_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_loa_accepted = () =>
        {
            result.ShouldBeTheSameAs("LOA Accepted");
        };
    }
}