using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesPaidThisYearSpecs
{
    public class when_handling_paid_event : WithFakes
    {
        private static IAttorneyInvoicesPaidThisYearDataManager attorneyInvoicesPaidThisMonthYearManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;
        private static AttorneyInvoicesPaidThisYearHandler handler;
        private static ThirdPartyInvoiceMarkedAsPaidEvent @event;
        private static int thirdPartyInvoiceKey;
        private static IServiceRequestMetadata metadata;
        private static decimal invoiceTotalValue;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;

        private Establish context = () =>
        {
            invoiceTotalValue = 1959.00M;
            thirdPartyInvoice = new ThirdPartyInvoiceDataModel(1, "sahl_reference", 1, 1428540, Guid.NewGuid(), "invoice_number", null, "clintons@sahomeloans.com", invoiceTotalValue, 0M, invoiceTotalValue, true,
               DateTime.Now, "payment_reference");
            breakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            metadata = An<IServiceRequestMetadata>();
            attorneyInvoicesPaidThisMonthYearManager = An<IAttorneyInvoicesPaidThisYearDataManager>();
            thirdPartyInvoiceKey = 12143;
            breakdownDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(thirdPartyInvoiceKey)).Return(thirdPartyInvoice);
            @event = new ThirdPartyInvoiceMarkedAsPaidEvent(DateTime.Now, thirdPartyInvoiceKey);
            handler = new AttorneyInvoicesPaidThisYearHandler(attorneyInvoicesPaidThisMonthYearManager, breakdownDataManager);
        };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_get_the_third_party_invoice_details = () =>
        {
            breakdownDataManager.WasToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(thirdPartyInvoiceKey));
        };

        private It should_increment_the_projection_using_the_data_manager = () =>
        {
            attorneyInvoicesPaidThisMonthYearManager.WasToldTo(x => x.IncrementCountAndAddInvoiceToValueColumn(invoiceTotalValue));
        };
    }
}