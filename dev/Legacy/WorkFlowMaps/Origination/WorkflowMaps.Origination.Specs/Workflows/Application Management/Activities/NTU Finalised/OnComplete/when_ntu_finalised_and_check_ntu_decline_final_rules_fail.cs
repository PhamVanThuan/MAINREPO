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
    internal class when_ntu_finalised_and_check_ntu_decline_final_rules_fail : WorkflowSpecApplicationManagement
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
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;

            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckNTUDeclineFinalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
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

        private It should_perform_check_ntu_decline_final_rules = () =>
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

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}