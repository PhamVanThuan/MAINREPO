using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Return_to_Manage_Application.OnComplete
{
    [Subject("Activities => Return_to_Manage_Application => OnComplete")]
    internal class when_return_to_manage_application : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Return_to_Manage_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_archive_v3itc_for_application = () =>
        {
            common.WasToldTo(x => x.ArchiveV3ITCForApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1));
        };

        private It should_create_new_revision = () =>
        {
            common.WasToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}