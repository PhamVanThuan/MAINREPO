using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.ReAssign_Commission_Consultant.OnGetActivityMessage
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnGetActivityMessage")]
    internal class when_reassign_commission_consultant : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}