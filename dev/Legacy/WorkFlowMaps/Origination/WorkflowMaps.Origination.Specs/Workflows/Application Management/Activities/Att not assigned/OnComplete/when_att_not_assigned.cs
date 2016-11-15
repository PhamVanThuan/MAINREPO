using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Att_not_assigned.OnComplete
{
    [Subject("Activity => Att_not_assigned => OnComplete")] // AutoGenerated
    internal class when_att_not_assigned : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Att_not_assigned(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}