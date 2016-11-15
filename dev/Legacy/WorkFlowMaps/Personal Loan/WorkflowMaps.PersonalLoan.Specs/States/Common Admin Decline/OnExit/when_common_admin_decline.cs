using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.States.Common_Admin_Decline.OnExit
{
    [Subject("State => Common_Admin_Decline => OnExit")] // AutoGenerated
    internal class when_common_admin_decline : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Admin_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}