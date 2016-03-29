using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesNotProcessedThisMonthSpecs
{
    public class when_handling_approved_event : WithFakes
    {
        private static InvoicesNotProcessedThisMonthApprovedHandler handler;
        private static ThirdPartyInvoiceApprovedEvent @event;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;

        private Establish context = () =>
         {
             metadata = An<IServiceRequestMetadata>();
             dataManager = An<IAttorneyInvoicesNotProcessedThisMonthDataManager>();
             var invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null, 1212, 1, 100M, false) };
              thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
             @event = new ThirdPartyInvoiceApprovedEvent(DateTime.Now, thirdPartyInvoiceModel, 5000M, @"SAHL\ClintonS", "Invoice Approver");
             handler = new InvoicesNotProcessedThisMonthApprovedHandler(dataManager);
         };

        private Because of = () =>
         {
             handler.Handle(@event, metadata);
         };

        private It should_increase_the_paid_count = () =>
         {
             dataManager.WasToldTo(x => x.DecrementCountAndDecreaseMonthlyValue(@event.ApprovedThirdPartyInvoice.TotalAmountIncludingVAT));
         };
    }
}