using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.CapitecDeclinedApplication.OnStart
{
    [Subject("Activity => CapitecDeclinedApplication => OnStart")] // AutoGenerated
    internal class when_capitecdeclinedapplication : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_CapitecDeclinedApplication(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}