using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities._12hrs.OnStart
{
    [Subject("Activity => 12hrs => OnStart")] // AutoGenerated
    internal class when_12hrs : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_12hrs(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}