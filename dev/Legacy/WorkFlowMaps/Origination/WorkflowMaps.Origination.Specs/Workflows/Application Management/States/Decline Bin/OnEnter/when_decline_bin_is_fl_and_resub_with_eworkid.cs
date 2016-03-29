using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Decline_Bin.OnEnter
{
    [Subject("States => Decline_Bin => OnEnter")]
    internal class when_decline_bin_is_fl_and_resub_with_eworkid : WorkflowSpecApplicationManagement
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
            workflowData.EWorkFolderID = "1";

            appMan.WhenToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName)).Return(true);
            common.WhenToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, workflowData.EWorkFolderID, SAHL.Common.Constants.EworkActionNames.X2DECLINEFINAL, workflowData.ApplicationKey,
                                                        paramsData.ADUserName, paramsData.StateName)).Return(true);
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

        private It should_archive_children = () =>
        {
            appMan.WasToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName));
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 5, -1));
        };

        private It should_perform_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, workflowData.EWorkFolderID, SAHL.Common.Constants.EworkActionNames.X2DECLINEFINAL, workflowData.ApplicationKey,
                                                       paramsData.ADUserName, paramsData.StateName));
        };

        private It should_send_final_resub_mail = () =>
        {
            appMan.WasToldTo(x => x.SendNTUFinalResubMail((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}