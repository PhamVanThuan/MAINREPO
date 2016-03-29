using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Decline_Bin.OnEnter
{
    [Subject("States => Decline_Bin => OnEnter")]
    internal class when_decline_bin_is_fl_and_does_not_archive_children : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;
        private static ICommon common;
        private static IFL fl;

        private Establish context = () =>
        {
            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            result = false;
            workflowData.IsFL = true;
            workflowData.IsResub = true;

            appMan.WhenToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName)).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Decline_Bin(instanceData, workflowData, paramsData, messages);
        };

        private It should_unhold_fl_applications = () =>
        {
            fl.WasToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_remove_detail_types = () =>
        {
            fl.WasToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_not_update_offer_status = () =>
        {
            common.WasNotToldTo(x => x.UpdateOfferStatus(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_perform_ework_action = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(),
                                                          Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_not_send_resub_mail = () =>
        {
            appMan.WasNotToldTo(x => x.SendNTUFinalResubMail(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };
    }
}