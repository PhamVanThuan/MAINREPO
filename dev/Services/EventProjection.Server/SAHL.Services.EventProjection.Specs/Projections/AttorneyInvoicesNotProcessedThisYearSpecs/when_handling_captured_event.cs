using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesNotProcessedThisYearSpecs
{
    public class when_handling_captured_event : WithFakes
    {
        private static InvoicesNotProcessedThisYearCapturedHandler handler;
        private static ThirdPartyInvoiceCapturedEvent @event;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoicesNotProcessedThisYearDataManager dataManager;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;

        private Establish context = () =>
         {
             metadata = An<IServiceRequestMetadata>();
             dataManager = An<IAttorneyInvoicesNotProcessedThisYearDataManager>();
             var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null, 1212, 1, 100M, false) };
              thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
             @event = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, thirdPartyInvoiceModel);
             handler = new InvoicesNotProcessedThisYearCapturedHandler(dataManager);
         };

        private Because of = () =>
         {
             handler.Handle(@event, metadata);
         };

        private It should_increase_the_paid_count = () =>
         {
             dataManager.WasToldTo(x => x.IncrementCountAndIncreaseYearlyValue(@event.ThirdPartyInvoiceModel.TotalAmountIncludingVAT));
         };
    }
}