using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.isAccidentalCover.OnStart
{
    [Subject("Activity => is_Accidental_Cover => OnStart")]
    internal class when_is_not_accidental_cover : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.PolicyTypeKey = (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_isAccidentalCover(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}