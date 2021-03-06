using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Create_Instance.OnStart
{
    [Subject("Activity => Create_Instance => OnStart")] // AutoGenerated
    internal class when_create_instance : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}