using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Assign_TeleConsultant.OnExit
{
    [Subject("State => Assign_TeleConsultant => OnEnter")]
    internal class when_assign_teleconsultant : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Assign_TeleConsultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}