using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirmation_Required.OnComplete
{
    [Subject("Activity => Confirmation_Required => OnComplete")]
    internal class when_confirmation_not_required : WorkflowSpecLife
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
            {
                result = false;
                workflowData.FAISDone = 0;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Confirmation_Not_Required(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_set_the_fais_done_data_property_to_true = () =>
            {
                workflowData.FAISDone.ShouldEqual(1);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}