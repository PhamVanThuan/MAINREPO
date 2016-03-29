using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.isStandardCover.OnStart
{
    [Subject("Activity => is_Standard_Cover => OnStart")]
    internal class when_is_not_standard_cover : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.PolicyTypeKey = (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_isStandardCover(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}