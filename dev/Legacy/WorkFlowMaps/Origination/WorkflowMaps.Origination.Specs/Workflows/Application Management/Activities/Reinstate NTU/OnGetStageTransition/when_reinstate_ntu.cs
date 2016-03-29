using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_NTU.OnGetStageTransition
{
    [Subject("Activity => Reinstate_NTU => OnGetStageTransition")]
    internal class when_reinstate_ntu : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_reinstate_ntu = () =>
        {
            result.ShouldEqual("Reinstate NTU");
        };
    }
}