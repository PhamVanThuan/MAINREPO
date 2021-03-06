using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.ReAssign_Commission_Consultant.OnStart
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnStart")] // AutoGenerated
    internal class when_reassign_commission_consultant : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}