using Machine.Specifications;
using SAHL.WorkflowMaps.LifeClaims.Specs;

namespace WorkflowMaps.DisabilityClaim.Specs.Activities.Approve.OnStart
{
    [Subject("Activity => Approve => OnStart")] // AutoGenerated
    internal class when_approve : WorkflowSpecDisabilityClaim
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_Approve(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}