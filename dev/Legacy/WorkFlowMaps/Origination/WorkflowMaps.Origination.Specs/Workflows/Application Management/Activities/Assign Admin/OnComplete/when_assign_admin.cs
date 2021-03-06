using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Assign_Admin.OnComplete
{
    [Subject("Activity => Assign_Admin => OnComplete")] // AutoGenerated
    internal class when_assign_admin : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Assign_Admin(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}