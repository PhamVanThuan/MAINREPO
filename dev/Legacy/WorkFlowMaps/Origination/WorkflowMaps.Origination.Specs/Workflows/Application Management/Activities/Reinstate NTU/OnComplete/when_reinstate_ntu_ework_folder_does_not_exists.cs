using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_NTU.OnComplete
{
    [Subject("Activity => Reinstate_NTU => OnComplete")]
    internal class when_reinstate_ntu_ework_folder_does_not_exists : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedAssignedTo;
        private static IWorkflowAssignment assignment;
        private static ICommon common;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedAssignedTo = "AssignedToTest";
            workflowData.EWorkFolderID = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            workflowData.PreviousState = "PreviousStateTest";
            workflowData.IsFL = true;
            workflowData.AppCapIID = 3;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_resolve_branch_consultant_d_dynamic_role_to_user_name = () =>
        {
            assignment.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Branch Consultant D", instanceData.ID));
        };

        private It should_not_perform_ework_action = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_handle_app_man_roles_on_return_from_ntu_to_previous_state = () =>
        {
            assignment.WasToldTo(x => x.HandleApplicationManagementRolesOnReturnFromNTUtoPreviousState((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                workflowData.PreviousState,
                workflowData.IsFL,
                workflowData.AppCapIID,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}