using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.InvoiceCapturedHandler
{
    public class when_handling_event : WithFakes
    {
        private static ThirdPartyInvoiceCapturedEvent @event;
        private static ThirdPartyInvoiceCapturedHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;

        private Establish that = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            metadata = new ServiceRequestMetadata();
            var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) };
            ThirdPartyInvoiceModel thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            @event = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, thirdPartyInvoiceModel);
            projector = new ThirdPartyInvoiceCapturedHandler(attorneyInvoiceMonthlyBreakdownManager);
        };

        private Because of = () =>
            {
                projector.Handle(@event, metadata);
            };

        private It should_ensure_a_projection_record_exists_for_the_attorney = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(a => a.EnsureProjectionRecordIsCreatedForAttorney(@event.ThirdPartyInvoiceModel.ThirdPartyId))
               .OnlyOnce();
        };

        private It should_increment_the_processed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.Received().IncrementUnProcessedCountForAttorney(@event.ThirdPartyInvoiceModel.ThirdPartyId);
        };
    }
}