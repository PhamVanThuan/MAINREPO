using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Manager_Review.OnExit
{
    [Subject("State => Manager_Review => OnExit")] // AutoGenerated
    internal class when_manager_review : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Manager_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}