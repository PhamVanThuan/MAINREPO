using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Readvance_Payments.OnStart
{
    [Subject("Activity => Readvance_Payments => OnStart")] // AutoGenerated
    internal class when_readvance_payments : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Readvance_Payments(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}