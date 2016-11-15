using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Create_Followup.OnStart
{
    [Subject("Activity => Create_Followup => OnStart")] // AutoGenerated
    internal class when_create_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}