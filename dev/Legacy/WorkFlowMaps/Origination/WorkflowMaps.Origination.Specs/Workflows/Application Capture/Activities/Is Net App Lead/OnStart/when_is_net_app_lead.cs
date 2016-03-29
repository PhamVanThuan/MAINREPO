using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Is_Net_App_Lead.OnStart
{
    [Subject("Activity => Is_Net_App_Lead => OnStart")] // AutoGenerated
    internal class when_is_net_app_lead : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Net_App_Lead(messages, workflowData, instanceData, paramsData);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}