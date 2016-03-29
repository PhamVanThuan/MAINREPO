using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.EXT_Create_Net_Lead.OnComplete
{
    [Subject("Activity => EXT_Create_Net_Lead => OnComplete")]
    internal class when_ext_create_net_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.LeadType = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Create_Net_Lead(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_lead_type_data_parameter_to_3 = () =>
        {
            workflowData.LeadType.ShouldEqual(3);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}