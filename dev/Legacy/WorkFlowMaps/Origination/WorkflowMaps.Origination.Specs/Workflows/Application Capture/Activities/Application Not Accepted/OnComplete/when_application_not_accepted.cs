using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Application_Not_Accepted.OnComplete
{
    [Subject("Activity => Application_Not_Accepted => OnComplete")]
    internal class when_application_not_accepted : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static string expectedLastStateName;
        private static string expectedLastNTUStateName;

        private Establish context = () =>
        {
            workflowData.Last_State = String.Empty;
            workflowData.Last_NTU_State = String.Empty;
            ((ParamsDataStub)paramsData).StateName = "Contact Client";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Application_Not_Accepted(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_last_state_data_property_to_instance_data_state_name = () =>
        {
            workflowData.Last_State.ShouldMatch(paramsData.StateName);
        };

        private It should_set_last_ntu_state_data_property_to_instance_data_state_name = () =>
        {
            workflowData.Last_NTU_State.ShouldMatch(paramsData.StateName);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}