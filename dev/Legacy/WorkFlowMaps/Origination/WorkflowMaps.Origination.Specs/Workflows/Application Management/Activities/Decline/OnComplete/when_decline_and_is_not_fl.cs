using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline.OnComplete
{
    [Subject("Activity => Decline => OnComplete")]
    internal class when_decline_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = false;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.AppCapIID = 2;
            workflowData.ApplicationKey = 3;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_reactive_branch_user_for_origination = () =>
        {
            assignment.WasToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.AppCapIID,
                workflowData.ApplicationKey,
                "Decline",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}