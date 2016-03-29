using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_Exclusions.OnComplete
{
    [Subject("Activity => Accept_Exclusions => OnComplete")]
    internal class when_accept_exclusions : WorkflowSpecLife
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ExclusionsDone = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Accept_Exclusions(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_exclusions_done_data_property = () =>
        {
            workflowData.ExclusionsDone.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}