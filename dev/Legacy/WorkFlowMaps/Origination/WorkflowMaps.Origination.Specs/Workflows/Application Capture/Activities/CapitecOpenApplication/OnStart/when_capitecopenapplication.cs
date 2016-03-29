using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.CapitecOpenApplication.OnStart
{
    [Subject("Activity => CapitecOpenApplication => OnStart")] // AutoGenerated
    internal class when_capitecopenapplication : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_CapitecOpenApplication(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}