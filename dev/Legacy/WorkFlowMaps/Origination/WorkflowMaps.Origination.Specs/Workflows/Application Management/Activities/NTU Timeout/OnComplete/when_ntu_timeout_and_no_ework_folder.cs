using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Timeout.OnComplete
{
    [Subject("Activity => NTU_Timeout => OnComplete")]
    internal class when_ntu_timeout_and_no_ework_folder : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IWorkflowAssignment assignment;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.EWorkFolderID = string.Empty;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
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

        private It should_not_resolve_dynamic_role_to_user_name = () =>
        {
            assignment.WasNotToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()));
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

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}