using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Clear_Cache.OnStart
{
    [Subject("Activity => Clear_Cache => OnStart")] // AutoGenerated
    internal class when_clear_cache : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Clear_Cache(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}