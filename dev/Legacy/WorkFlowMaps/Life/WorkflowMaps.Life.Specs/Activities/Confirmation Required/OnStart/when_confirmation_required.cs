using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirmation_Required.OnStart
{
    [Subject("Activity => Confirmation_Required => OnStart")]
    internal class when_confirmation_required : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.ConfirmationRequired = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Confirmation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}