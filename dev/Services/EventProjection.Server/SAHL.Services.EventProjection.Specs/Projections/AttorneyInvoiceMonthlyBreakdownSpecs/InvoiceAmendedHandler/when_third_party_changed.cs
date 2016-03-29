using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.InvoiceAmendedHandler
{
    public class when_third_party_changed : WithFakes
    {
        private static ThirdPartyInvoiceAmendedEvent @event;
        private static ThirdPartyInvoiceAmendedHandler projector;
        private static IServiceRequestMetadata metadata;
        private static Guid newThirdParty;
        private static Guid oldThirdParty;
        private static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;

        private Establish that = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            projector = new ThirdPartyInvoiceAmendedHandler(attorneyInvoiceMonthlyBreakdownManager);
            newThirdParty = Guid.NewGuid();
            oldThirdParty = Guid.NewGuid();
            metadata = new ServiceRequestMetadata();
            var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) };
            var newThirdPartyInvoice = new ThirdPartyInvoiceModel(1212, newThirdParty, "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            var OldThirdPartyInvoice = new ThirdPartyInvoiceDataModel("reference", 111210, 112, oldThirdParty, "FF0011", DateTime.Now, null, 100.00M, 14.00M, 114.00M,
                true, DateTime.Now, string.Empty);
            @event = new ThirdPartyInvoiceAmendedEvent(DateTime.Now, OldThirdPartyInvoice, newThirdPartyInvoice, null, null, null);
        };

        private Because of = () =>
            {
                projector.Handle(@event, metadata);
            };

        private It Should_decrement_unprocessed_for_the_old_attorny = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.DecrementUnProcessedCountForAttorney(oldThirdParty));
        };

        private It Should_update_the_attorney_invoice_monthly_breakdown_projection_for_new_attorny = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.EnsureProjectionRecordIsCreatedForAttorney(@event.AmmendedThirdPartyInvoice.ThirdPartyId));
        };
    }
}