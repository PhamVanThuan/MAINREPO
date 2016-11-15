using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Common_Re_Assign.OnExit
{
    [Subject("State => Common_Re_Assign => OnExit")] // AutoGenerated
    internal class when_common_re_assign : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Re_Assign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}