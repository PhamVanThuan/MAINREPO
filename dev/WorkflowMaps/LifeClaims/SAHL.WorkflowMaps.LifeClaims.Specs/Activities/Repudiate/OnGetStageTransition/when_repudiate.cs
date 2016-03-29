using Machine.Specifications;
using SAHL.WorkflowMaps.LifeClaims.Specs;

namespace WorkflowMaps.DisabilityClaim.Specs.Activities.Repudiate.OnGetStageTransition
{
    [Subject("Activity => Repudiate => OnGetStageTransition")] // AutoGenerated
    internal class when_repudiate : WorkflowSpecDisabilityClaim
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_Repudiate(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}