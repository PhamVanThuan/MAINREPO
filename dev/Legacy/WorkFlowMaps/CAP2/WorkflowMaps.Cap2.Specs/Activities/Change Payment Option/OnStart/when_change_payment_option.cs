using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Change_Payment_Option.OnStart
{
    [Subject("Activity => Change_Payment_Option => OnStart")] // AutoGenerated
    internal class when_change_payment_option : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Change_Payment_Option(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}