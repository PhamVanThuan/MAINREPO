using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reassign_User.OnGetActivityMessage
{
    [Subject("Activity => Reassign_User => OnGetActivityMessage")]
    internal class when_reassign_user : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}