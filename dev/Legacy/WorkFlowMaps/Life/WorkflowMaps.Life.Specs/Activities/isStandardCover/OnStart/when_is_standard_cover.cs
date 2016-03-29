using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.isStandardCover.OnStart
{
    [Subject("Activity => is_Standard_Cover => OnStart")]
    internal class when_is_standard_cover : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PolicyTypeKey = (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_isStandardCover(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}