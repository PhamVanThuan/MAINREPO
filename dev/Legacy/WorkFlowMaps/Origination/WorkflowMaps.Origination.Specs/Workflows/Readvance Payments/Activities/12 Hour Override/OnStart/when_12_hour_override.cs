using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities._12_Hour_Override.OnStart
{
    [Subject("Activity => 12_Hour_Override => OnStart")] // AutoGenerated
    internal class when_12_hour_override : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_12_Hour_Override(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}