using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Application_Not_Accepted.OnReturn
{
    [Subject("State => Application_Not_Accepted => OnReturn")] // AutoGenerated
    internal class when_application_not_accepted : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Application_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}