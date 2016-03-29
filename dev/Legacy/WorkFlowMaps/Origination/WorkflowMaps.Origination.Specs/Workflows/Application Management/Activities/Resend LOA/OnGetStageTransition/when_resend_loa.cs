using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resend_LOA.OnGetStageTransition
{
    [Subject("Activity => Resend_LOA => OnGetStageTransition")]
    internal class when_resend_loa : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Resend_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_resend_loa = () =>
        {
            result.ShouldEqual("Resend LOA");
        };
    }
}