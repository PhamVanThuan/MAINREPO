using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Finalised.OnGetStageTransition
{
    [Subject("Activity => OnGetStageTransition => OnStart")]
    internal class when_ntu_finalised : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_ntu_finalised = () =>
        {
            result.ShouldMatch("NTU Finalised");
        };
    }
}