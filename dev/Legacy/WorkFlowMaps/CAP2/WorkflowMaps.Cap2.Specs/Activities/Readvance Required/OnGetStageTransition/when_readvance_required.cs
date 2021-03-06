using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Readvance_Required.OnGetStageTransition
{
    [Subject("Activity => Readvance_Required => OnGetStageTransition")] // AutoGenerated
    internal class when_readvance_required : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Readvance_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}