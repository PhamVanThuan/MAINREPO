using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Is_Net_App_Lead.OnStart
{
    [Subject("Activity => Is_Net_App_Lead => OnStart")] // AutoGenerated
    internal class when_is_net_app_lead_and_lead_type_is_4 : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.LeadType = 4;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Net_App_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}