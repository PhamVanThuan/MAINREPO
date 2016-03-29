using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Create_EWork_PipelineCase.OnGetStageTransition
{
    [Subject("Activity => Create_EWork_PipelineCase => OnGetStageTransition")] // AutoGenerated
    internal class when_create_ework_pipelinecase : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_EWork_PipelineCase(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}