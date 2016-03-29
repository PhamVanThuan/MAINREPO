using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Campaign_Instance.OnComplete
{
    [Subject("Activity => Create_Campaign_Instance => OnComplete")]
    internal class when_create_campaign_instance : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static int expectedLeadType;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            expectedLeadType = 2;//Campaign, there is no lookup for this have to look at map to determine what 2 means
            workflowData.LeadType = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Campaign_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_lead_type_data_property_to_estate_agent_consultant = () =>
        {
            workflowData.LeadType.ShouldEqual<int>(expectedLeadType);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}