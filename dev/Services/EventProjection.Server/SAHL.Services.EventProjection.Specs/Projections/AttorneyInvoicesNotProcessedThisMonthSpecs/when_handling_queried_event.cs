using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesNotProcessedThisMonthSpecs
{
    public class when_handling_queried_event : WithFakes
    {
        private static InvoicesNotProcessedThisMonthQueriedHandler handler;
        private static ThirdPartyInvoiceQueriedPostApprovalEvent @event;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoiceModel;
        private static decimal invoiceValue;

        private Establish context = () =>
         {
             invoiceValue = 1122;
             metadata = An<IServiceRequestMetadata>();
             breakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
             dataManager = An<IAttorneyInvoicesNotProcessedThisMonthDataManager>();
             thirdPartyInvoiceModel = new ThirdPartyInvoiceDataModel("reference", 111210, 112, Guid.NewGuid(), "FF0011", DateTime.Now, null, invoiceValue, 0.00M, invoiceValue,
                true, DateTime.Now, string.Empty);
             @event = new ThirdPartyInvoiceQueriedPostApprovalEvent(DateTime.Now, 123456, "ClintonS", "comments");
             breakdownDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey)).Return(thirdPartyInvoiceModel);
             handler = new InvoicesNotProcessedThisMonthQueriedHandler(dataManager, breakdownDataManager);
         };

        private Because of = () =>
         {
             handler.Handle(@event, metadata);
         };

        private It should_fetch_the_invoice_details = () =>
         {
             breakdownDataManager.WasToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey));
         };

        private It should_increment_the_unprocessed_count = () =>
        {
            dataManager.WasToldTo(x => x.IncrementCountAndIncreaseMonthlyValue(invoiceValue));
        };
    }
}