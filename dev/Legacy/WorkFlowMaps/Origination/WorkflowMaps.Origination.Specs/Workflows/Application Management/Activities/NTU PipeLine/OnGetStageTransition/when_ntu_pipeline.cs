using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_PipeLine.OnGetStageTransition
{
    [Subject("Activity => NTU_PipeLine => OnStart")]
    internal class when_ntu_pipeline : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU_PipeLine(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_ntu_pipeLine = () =>
        {
            result.ShouldMatch("NTU PipeLine");
        };
    }
}