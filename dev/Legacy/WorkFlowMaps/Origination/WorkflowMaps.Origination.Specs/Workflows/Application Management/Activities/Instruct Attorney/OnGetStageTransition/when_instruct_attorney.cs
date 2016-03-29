using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Instruct_Attorney.OnGetStageTransition
{
    [Subject("Activity => Instruct_Attorney => OnGetStageTransition")]
    internal class when_instruct_attorney : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Instruct_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_instruct_attorney = () =>
        {
            result.ShouldBeTheSameAs("Instruct Attorney");
        };
    }
}