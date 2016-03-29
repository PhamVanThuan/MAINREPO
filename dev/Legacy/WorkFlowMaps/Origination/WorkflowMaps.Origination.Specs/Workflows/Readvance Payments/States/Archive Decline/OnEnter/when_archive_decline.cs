using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Archive_Decline.OnEnter
{
    [Subject("State => Archive_Decline => OnEnter")]
    internal class when_archive_decline : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL fl;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_remove_detail_types = () =>
        {
            fl.WasToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_archive_related_cases = () =>
        {
            fl.WasToldTo(x => x.ArchiveFLRelatedCases((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Declined, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}