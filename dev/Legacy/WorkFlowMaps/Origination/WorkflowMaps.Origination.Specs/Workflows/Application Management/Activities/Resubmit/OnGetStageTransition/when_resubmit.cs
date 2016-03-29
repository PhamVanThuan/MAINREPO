using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resubmit.OnGetStageTransition
{
    [Subject("Activity => Resubmit => OnGetStageTransition")]
    internal class when_resubmit : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Resubmit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_resubmit = () =>
        {
            result.ShouldEqual("Resubmit");
        };
    }
}