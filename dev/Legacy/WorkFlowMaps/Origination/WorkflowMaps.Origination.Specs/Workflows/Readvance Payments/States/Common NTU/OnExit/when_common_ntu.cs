using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Common_NTU.OnExit
{
    [Subject("State => Common_NTU => OnExit")] // AutoGenerated
    internal class when_common_ntu : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}