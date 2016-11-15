using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Decline_Bin.OnReturn
{
    [Subject("State => Decline_Bin => OnReturn")] // AutoGenerated
    internal class when_decline_bin : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Decline_Bin(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}