using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Readvance_Required.OnExit
{
    [Subject("State => Readvance_Required => OnExit")] // AutoGenerated
    internal class when_readvance_required : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Readvance_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}