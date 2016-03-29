using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.QuickCash_Hold.OnEnter
{
    [Subject("State => QuickCash_Hold => OnEnter")] // AutoGenerated
    internal class when_quickcash_hold : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_QuickCash_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}