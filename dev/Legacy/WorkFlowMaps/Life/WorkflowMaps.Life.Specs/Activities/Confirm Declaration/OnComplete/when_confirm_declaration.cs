using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_Declaration.OnComplete
{
    [Subject("Activity => Confirm_Declaration => OnComplete")]
    internal class when_confirm_declaration : WorkflowSpecLife
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                message = string.Empty;
                result = false;
                workflowData.DeclarationConfirmationDone = 0;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Confirm_Declaration(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_set_the_declaration_confirmation_done_data_property = () =>
            {
                workflowData.DeclarationConfirmationDone.ShouldEqual(1);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}