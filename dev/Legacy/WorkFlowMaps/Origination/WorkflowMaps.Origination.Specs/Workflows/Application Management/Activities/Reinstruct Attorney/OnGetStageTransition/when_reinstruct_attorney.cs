using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstruct_Attorney.OnGetStageTransition
{
    [Subject("Activity => Reinstruct_Attorney => OnGetStageTransition")]
    internal class when_reinstruct_attorney : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reinstruct_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_reinstruct_attorney = () =>
        {
            result.ShouldEqual("Reinstruct Attorney");
        };
    }
}