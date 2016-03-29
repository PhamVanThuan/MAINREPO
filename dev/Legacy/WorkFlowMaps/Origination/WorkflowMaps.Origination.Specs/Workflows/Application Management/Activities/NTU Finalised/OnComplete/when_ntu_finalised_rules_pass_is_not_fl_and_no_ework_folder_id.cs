using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Finalised.OnComplete
{
    [Subject("Activity => NTU_Finalised => OnComplete")]
    internal class when_ntu_finalised_rules_pass_is_not_fl_and_no_ework_folder_id : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.IsFL = false;
            workflowData.EWorkFolderID = string.Empty;
            workflowData.ApplicationKey = 1;

            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckNTUDeclineFinalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU_Finalised(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_ntu_decline_final = () =>
        {
            fl.WasToldTo(x => x.CheckNTUDeclineFinalRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_not_remove_detail_from_application_after_ntu_finalised = () =>
        {
            appMan.WasNotToldTo(x => x.RemoveDetailFromApplicationAfterNTUFinalised(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_not_perform_ework_action_x2_ntu_advise = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}