using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.AssignConsultant.OnStart
{
    [Subject("Activity => AssignConsultant => OnStart")] // AutoGenerated
    internal class when_assignconsultant : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AssignConsultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}