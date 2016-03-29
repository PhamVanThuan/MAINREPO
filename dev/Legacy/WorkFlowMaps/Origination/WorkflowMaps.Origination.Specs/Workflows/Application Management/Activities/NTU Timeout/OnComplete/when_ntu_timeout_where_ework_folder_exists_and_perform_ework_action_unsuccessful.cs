using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Timeout.OnComplete
{
    [Subject("Activity => NTU_Timeout => OnComplete")]
    internal class when_ntu_timeout_where_ework_folder_exists_and_perform_ework_action_unsuccessful : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedAssignedTo;
        private static ICommon common;
        private static IWorkflowAssignment assignment;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            expectedAssignedTo = "AssignedToTest";
            workflowData.EWorkFolderID = "EWorkFolderIDTest";
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;
            ((ParamsDataStub)paramsData).StateName = "StateNameTest";
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";

            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU_Timeout(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_add_reason_description_expiry_due_to_excessive_time_lapse_and_type_application_ntu = () =>
        {
            common.WasToldTo(x => x.AddReasons((IDomainMessageCollection)messages, workflowData.ApplicationKey, 182, (int)SAHL.Common.Globals.ReasonTypes.ApplicationNTU));
        };

        private It should_resolve_branch_consultant_d_dynamic_role_to_user_name = () =>
        {
            assignment.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Branch Consultant D", instanceData.ID));
        };

        private It should_perform_x2_client_refused_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2ClientRefused,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}