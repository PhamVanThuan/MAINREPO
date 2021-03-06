using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Check_Source_and_LeadType.OnExit
{
    [Subject("State => Check_Source_and_LeadType => OnExit")] // AutoGenerated
    internal class when_check_source_and_leadtype : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Check_Source_and_LeadType(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}