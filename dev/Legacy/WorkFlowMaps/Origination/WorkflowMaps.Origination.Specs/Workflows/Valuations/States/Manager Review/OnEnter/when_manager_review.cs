using Machine.Specifications;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.States.Manager_Review.OnEnter
{
    [Subject("State => Manager_Review => OnEnter")]
    internal class when_manager_review : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.OnManagerWorkList = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manager_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_on_manager_worklist_to_true = () =>
        {
            workflowData.OnManagerWorkList.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}