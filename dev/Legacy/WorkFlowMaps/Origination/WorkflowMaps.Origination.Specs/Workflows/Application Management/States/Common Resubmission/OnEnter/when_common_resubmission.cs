using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Resubmission.OnEnter
{
    [Subject("State => Common_Resubmission => OnEnter")] // AutoGenerated
    internal class when_common_resubmission : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Resubmission(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}