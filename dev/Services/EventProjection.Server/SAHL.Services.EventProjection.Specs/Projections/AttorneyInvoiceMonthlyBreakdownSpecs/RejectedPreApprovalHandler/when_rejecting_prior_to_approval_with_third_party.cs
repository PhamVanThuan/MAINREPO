using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.RejectedPreApprovalHandler
{
    public class when_rejecting_prior_to_approval_with_third_party : WithFakes
    {
        private static ThirdPartyInvoiceRejectedPreApprovalEvent @event;
        private static ThirdPartyInvoiceRejectedPreApprovalHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static Guid? thirdPartyid;

        private Establish that = () =>
        {
            thirdPartyid = Guid.NewGuid();
            attorneyInvoiceMonthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            metadata = new ServiceRequestMetadata();
            var OldThirdPartyInvoice = new ThirdPartyInvoiceDataModel("reference", 111210, 112, thirdPartyid, "FF0011", DateTime.Now, null, 100.00M, 14.00M, 114.00M,
                true, DateTime.Now, string.Empty);

            @event = new ThirdPartyInvoiceRejectedPreApprovalEvent(DateTime.Now, OldThirdPartyInvoice.AccountKey, null, OldThirdPartyInvoice.InvoiceNumber, null, "Rejected"
                , OldThirdPartyInvoice.SahlReference, OldThirdPartyInvoice.ThirdPartyInvoiceKey, "Subject", OldThirdPartyInvoice.ThirdPartyId);
            projector = new ThirdPartyInvoiceRejectedPreApprovalHandler(attorneyInvoiceMonthlyBreakdownManager);
        };

        private Because of = () =>
        {
            projector.Handle(@event, metadata);
        };

        private It should_decrement_the_unprocessed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.DecrementUnProcessedCountForAttorney(@event.ThirdPartyId.Value));
        };

        private It should_increment_the_rejected_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.IncrementRejectedCountForAttorney(@event.ThirdPartyId.Value));
        };
    }
}