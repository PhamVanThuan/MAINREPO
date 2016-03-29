using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_FAIS.OnComplete
{
    [Subject("Activity => Confirm_FAIS => OnComplete")]
    internal class when_confirm_fais : WorkflowSpecLife
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                result = false;
                message = string.Empty;
                workflowData.FAISConfirmationDone = 0;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Confirm_FAIS(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_set_the_fais_confirmation_done_data_property_to_true = () =>
            {
                workflowData.FAISConfirmationDone.ShouldEqual(1);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}