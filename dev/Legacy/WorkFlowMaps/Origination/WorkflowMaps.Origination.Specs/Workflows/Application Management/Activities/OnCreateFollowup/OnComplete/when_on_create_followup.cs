using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.OnCreateFollowup.OnComplete
{
    [Subject("Activity => OnCreateFollowup => OnComplete")]
    internal class when_on_create_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            workflowData.AppCapIID = 2;
            ((InstanceDataStub)instanceData).ID = 3;
            ((InstanceDataStub)instanceData).ID = 4;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            instanceData.ParentInstanceID = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_OnCreateFollowup(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_clone_active_security_from_instance_for_instance = () =>
        {
            assignment.WasToldTo(x => x.CloneActiveSecurityFromInstanceForInstance((IDomainMessageCollection)messages, (long)instanceData.ParentInstanceID, instanceData.ID));
        };

        private It should_reactive_branch_users_for_origination = () =>
        {
            assignment.WasToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.AppCapIID,
                workflowData.ApplicationKey,
                "OnCreateFollowup",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}