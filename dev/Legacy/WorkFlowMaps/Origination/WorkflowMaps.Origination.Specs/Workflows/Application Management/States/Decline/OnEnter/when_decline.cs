using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Decline.OnEnter
{
    [Subject("States => Decline => OnEnter")]
    internal class when_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 5, -1));
        };

        private It should_send_email_to_consultant = () =>
        {
            appMan.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID, 2));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}