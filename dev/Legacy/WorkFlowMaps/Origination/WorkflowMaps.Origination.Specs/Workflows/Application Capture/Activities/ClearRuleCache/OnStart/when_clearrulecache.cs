using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.ClearRuleCache.OnStart
{
    [Subject("Activity => ClearRuleCache => OnStart")] // AutoGenerated
    internal class when_clearrulecache : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_ClearRuleCache(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}