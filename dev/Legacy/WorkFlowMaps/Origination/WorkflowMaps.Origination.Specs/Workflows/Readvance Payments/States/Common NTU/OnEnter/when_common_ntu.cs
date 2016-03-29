using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Common_NTU.OnEnter
{
    [Subject("State => Common_NTU => OnEnter")] // AutoGenerated
    internal class when_common_ntu : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}