using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.ConsultantAssigned.OnStart
{
    [Subject("Activity => ConsultantAssigned => OnStart")] // AutoGenerated
    internal class when_consultantassigned : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_ConsultantAssigned(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}