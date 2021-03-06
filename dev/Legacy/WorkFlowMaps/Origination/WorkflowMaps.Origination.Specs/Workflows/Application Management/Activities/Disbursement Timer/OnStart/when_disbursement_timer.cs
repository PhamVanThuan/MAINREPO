using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Disbursement_Timer.OnStart
{
    [Subject("Activity => Disbursement_Timer => OnStart")] // AutoGenerated
    internal class when_disbursement_timer : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Disbursement_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}