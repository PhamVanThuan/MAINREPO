using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Branch_Rework_Application.OnComplete
{
    [Subject("Activity => Branch_Rework_Application => OnComplete")] // AutoGenerated
    internal class when_branch_rework_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Branch_Rework_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}