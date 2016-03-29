using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resubmit_to_Credit.OnGetStageTransition
{
    [Subject("Activity => Resubmit_to_Credit => OnGetStageTransition")]
    internal class when_resubmit_to_credit : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Resubmit_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_resubmit_to_credit = () =>
        {
            result.ShouldEqual("Resubmit to Credit");
        };
    }
}