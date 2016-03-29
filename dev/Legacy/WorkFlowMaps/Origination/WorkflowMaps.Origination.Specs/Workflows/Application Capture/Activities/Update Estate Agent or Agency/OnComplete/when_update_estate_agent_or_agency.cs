using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Workflows.Application_Capture.Activities.Update_Estate_Agent_or_Agency.OnComplete
{
    [Subject("Activity => Update_Estate_Agent_or_Agency => OnComplete")]
    internal class when_update_estate_agent_or_agency : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.isEstateAgentApplication = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Update_Estate_Agent_or_Agency(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_is_estate_agent_application_data_property_to_true = () =>
        {
            workflowData.isEstateAgentApplication.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}