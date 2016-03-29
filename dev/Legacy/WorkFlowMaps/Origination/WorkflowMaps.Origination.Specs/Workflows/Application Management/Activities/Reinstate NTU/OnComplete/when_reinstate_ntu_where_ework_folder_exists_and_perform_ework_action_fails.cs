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
    internal class when_reinstate_ntu_where_ework_folder_exists_and_perform_ework_action_fails : WorkflowSpecApplicationManagement
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
            workflowData.EWorkFolderID = "EWorkFolderID";
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            ((ParamsDataStub)paramsData).StateName = "StateNameTest";
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
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

        private It should_perform_x2_client_won_over_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2ClientWonOver,
                workflowData.ApplicationKey,
                expectedAssignedTo,
                paramsData.StateName));
        };

        private It should_not_handle_app_man_roles_on_return_from_ntu_to_previous_state = () =>
        {
            assignment.WasNotToldTo(x => x.HandleApplicationManagementRolesOnReturnFromNTUtoPreviousState(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<bool>(),
                Param.IsAny<long>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}