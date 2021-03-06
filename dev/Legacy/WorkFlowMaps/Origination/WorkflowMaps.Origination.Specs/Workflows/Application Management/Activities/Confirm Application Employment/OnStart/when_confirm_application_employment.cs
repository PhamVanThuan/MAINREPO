using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Confirm_Application_Employment.OnStart
{
    [Subject("Activity => Confirm_Application_Employment => OnStart")] // AutoGenerated
    internal class when_confirm_application_employment : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_Confirm_Application_Employment(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}