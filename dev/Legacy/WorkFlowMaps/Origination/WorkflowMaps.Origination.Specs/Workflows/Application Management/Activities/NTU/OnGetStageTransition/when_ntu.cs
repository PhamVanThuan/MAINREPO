using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU.OnGetStageTransition
{
    [Subject("Activity => NTU => OnGetStageTransition")]
    internal class when_ntu : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_ntu = () =>
        {
            result.ShouldMatch("NTU");
        };
    }
}