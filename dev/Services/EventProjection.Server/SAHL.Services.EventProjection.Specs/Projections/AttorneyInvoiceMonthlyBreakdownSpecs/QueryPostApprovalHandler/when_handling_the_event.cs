using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.QueryPostApprovalHandler
{
    public class when_handling_the_event : WithFakes
    {
        private static ThirdPartyInvoiceQueriedPostApprovalEvent @event;
        private static ThirdPartyInvoiceQueriedPostApprovalHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static Guid thirdPartyId;
        private static int thirdPartyInvoiceKey;

        private Establish that = () =>
        {
            thirdPartyInvoiceKey = 123312;
            thirdPartyId = Guid.NewGuid();
            attorneyInvoiceMonthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            metadata = An<IServiceRequestMetadata>();
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            @event = new ThirdPartyInvoiceQueriedPostApprovalEvent(DateTime.Now, thirdPartyInvoiceKey, "ClintonS", "Comments");
            attorneyInvoiceMonthlyBreakdownDataManager.WhenToldTo(x => x.GetThirdPartyIdByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey)).Return(thirdPartyId);
            projector = new ThirdPartyInvoiceQueriedPostApprovalHandler(attorneyInvoiceMonthlyBreakdownManager, attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
        {
            projector.Handle(@event, metadata);
        };

        private It should_retrieve_the_third_party_id = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.WasToldTo(x => x.GetThirdPartyIdByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey));
        };

        private It should_decrement_the_processed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.DecrementProcessedCountForAttorney(thirdPartyId));
        };

        private It should_increment_the_unprocessed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasToldTo(x => x.IncrementUnProcessedCountForAttorney(thirdPartyId));
        };
    }
}