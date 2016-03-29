using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.ApprovedHandler
{
    public class when_handling_event : WithFakes
    {
        private static ThirdPartyInvoiceApprovedEvent @event;
        private static ThirdPartyInvoiceApprovedHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownManager manager;

        private Establish that = () =>
        {
            manager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            metadata = new ServiceRequestMetadata();
            var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) };
            ThirdPartyInvoiceModel thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            @event = new ThirdPartyInvoiceApprovedEvent(DateTime.Now, thirdPartyInvoiceModel, 100.0M, null, null);
            projector = new ThirdPartyInvoiceApprovedHandler(manager);
        };

        private Because of = () =>
            {
                projector.Handle(@event, metadata);
            };

        private It Should_decrement_unprocessed_invoices = () =>
        {
            manager.WasToldTo(a => a.DecrementUnProcessedCountForAttorney(@event.ApprovedThirdPartyInvoice.ThirdPartyId))
               .OnlyOnce();
        };

        private It Should_increment_processed_invoice = () =>
        {
            manager.WasToldTo(a => a.IncrementProcessedCountForAttorney(@event.ApprovedThirdPartyInvoice.ThirdPartyId))
               .OnlyOnce();
        };
    }
}