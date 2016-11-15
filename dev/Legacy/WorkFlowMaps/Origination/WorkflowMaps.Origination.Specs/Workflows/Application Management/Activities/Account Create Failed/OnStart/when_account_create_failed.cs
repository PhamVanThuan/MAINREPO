using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Account_Create_Failed.OnStart
{
    [Subject("Activity => Account_Create_Failed => OnStart")] // AutoGenerated
    internal class when_account_create_failed : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Account_Create_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}