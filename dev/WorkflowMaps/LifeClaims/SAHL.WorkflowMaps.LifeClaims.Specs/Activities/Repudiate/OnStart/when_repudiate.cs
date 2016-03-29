using Machine.Specifications;
using SAHL.WorkflowMaps.LifeClaims.Specs;

namespace WorkflowMaps.DisabilityClaim.Specs.Activities.Repudiate.OnStart
{
    [Subject("Activity => Repudiate => OnStart")] // AutoGenerated
    internal class when_repudiate : WorkflowSpecDisabilityClaim
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_Repudiate(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}