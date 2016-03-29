using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesNotProcessedThisMonthSpecs
{
    public class when_handling_amended_event_with_adjustment : WithFakes
    {
        private static InvoicesNotProcessedThisMonthAmendedHandler handler;
        private static ThirdPartyInvoiceAmendedEvent @event;
        private static IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoiceModel;
        private static decimal adjustmentValue;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
         {
             metadata = An<IServiceRequestMetadata>();
             dataManager = An<IAttorneyInvoicesNotProcessedThisMonthDataManager>();
             var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null, 1212, 1, 1000.00M, true) };
             var amendedInvoice = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
             var originalInvoice = new ThirdPartyInvoiceDataModel("reference", 111210, 112, Guid.NewGuid(), "FF0011", DateTime.Now, null, 100.00M, 14.00M, 114.00M,
                 true, DateTime.Now, string.Empty);
             @event = new ThirdPartyInvoiceAmendedEvent(DateTime.Now, originalInvoice, amendedInvoice, null, null, null);
             adjustmentValue = @event.AmmendedThirdPartyInvoice.TotalAmountIncludingVAT - @event.OriginalThirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();

             handler = new InvoicesNotProcessedThisMonthAmendedHandler(dataManager);
         };

        private Because of = () =>
         {
             handler.Handle(@event, metadata);
         };

        private It should_decrement_the_unprocessed_value_amount = () =>
        {
            dataManager.WasToldTo(x => x.AdjustMonthlyValue(adjustmentValue));
        };
    }
}