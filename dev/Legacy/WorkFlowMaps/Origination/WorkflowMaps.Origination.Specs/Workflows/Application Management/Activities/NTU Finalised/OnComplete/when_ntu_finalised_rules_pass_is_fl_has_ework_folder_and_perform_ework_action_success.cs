using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Finalised.OnComplete
{
    [Subject("Activity => NTU_Finalised => OnComplete")]
    internal class when_ntu_finalised_rules_pass_is_fl_has_ework_folder_and_perform_ework_action_success : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;
        private static IFL fl;
        private static string expectedAssignedTo;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedAssignedTo = "AssignedToTest";
            workflowData.IsFL = true;
            workflowData.EWorkFolderID = "EWorkFolderIDTest";
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";

            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckNTUDeclineFinalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU_Finalised(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_check_ntu_decline_final_rules = () =>
        {
            fl.WasToldTo(x => x.CheckNTUDeclineFinalRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_remove_detail_from_application_after_ntu_finalised = () =>
        {
            appMan.WasToldTo(x => x.RemoveDetailFromApplicationAfterNTUFinalised((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_perform_ework_action_x2_ntu_advise = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2ClientRefused,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}