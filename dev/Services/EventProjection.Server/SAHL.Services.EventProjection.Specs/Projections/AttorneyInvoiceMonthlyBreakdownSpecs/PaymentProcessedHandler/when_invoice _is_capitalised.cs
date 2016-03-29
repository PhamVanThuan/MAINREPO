using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.PaymentProcessedHandler
{
    public class when_invoice__is_capitalised : WithFakes
    {
        private static ThirdPartyInvoiceMarkedAsPaidEvent @event;
        private static ThirdPartyInvoiceMarkedAsPaidHandler handler;
        private static int thirdPartyInvoiceKey;
        private static IAttorneyInvoiceMonthlyBreakdownManager monthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakdownDataManager;
        private static IServiceRequestMetadata metadata;
        private static ThirdPartyInvoiceDataModel invoice;
        private static decimal invoiceTotalValue;

        private Establish context = () =>
        {
            invoice = new ThirdPartyInvoiceDataModel(1, "sahl_reference", 1, 1428540, Guid.NewGuid(), "invoice_number", null, "clintons@sahomeloans.com", 5000.00M, 5000.00M, 5000.00M, true,
               DateTime.Now, "payment_reference");
            invoiceTotalValue = invoice.TotalAmountIncludingVAT.GetValueOrDefault();
            thirdPartyInvoiceKey = 148;
            @event = new ThirdPartyInvoiceMarkedAsPaidEvent(DateTime.Now, thirdPartyInvoiceKey);
            metadata = An<IServiceRequestMetadata>();
            monthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            monthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            monthlyBreakdownDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey)).Return(invoice);
            handler = new ThirdPartyInvoiceMarkedAsPaidHandler(monthlyBreakdownManager, monthlyBreakdownDataManager);
        };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_increment_the_paid_count = () =>
        {
            monthlyBreakdownDataManager.WasToldTo(x => x.IncrementPaidCount(invoice.ThirdPartyId.GetValueOrDefault()));
        };

        private It should_decrement_the_processed_count = () =>
        {
            monthlyBreakdownManager.WasToldTo(x => x.DecrementProcessedCountForAttorney(invoice.ThirdPartyId.GetValueOrDefault()));
        };

        private It should_increase_the_capitalised_value = () =>
        {
            monthlyBreakdownDataManager.WasToldTo(x => x.UpdatePaymentFieldsForAttorney(invoice.ThirdPartyId.GetValueOrDefault(), 0.00M,
                0.00M, invoiceTotalValue));
        };
    }
}