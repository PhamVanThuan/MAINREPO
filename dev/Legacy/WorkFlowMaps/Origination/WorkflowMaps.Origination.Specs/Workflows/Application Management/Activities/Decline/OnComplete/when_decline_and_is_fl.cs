using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline.OnComplete
{
    [Subject("Activity => Decline => OnComplete")]
    internal class when_decline_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = true;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_reactive_branch_user_for_origination = () =>
        {
            assignment.WasNotToldTo(x => x.ReActivateBranchUsersForOrigination(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}