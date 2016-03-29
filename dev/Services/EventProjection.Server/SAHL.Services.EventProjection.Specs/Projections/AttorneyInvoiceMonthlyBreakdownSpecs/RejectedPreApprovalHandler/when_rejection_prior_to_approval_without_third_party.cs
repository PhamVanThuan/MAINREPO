using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.RejectedPreApprovalHandler
{
    public class when_third_party_does_not_exist : WithFakes
    {
        private static ThirdPartyInvoiceRejectedPreApprovalEvent @event;
        private static ThirdPartyInvoiceRejectedPreApprovalHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static Guid? thirdPartyid;

        private Establish that = () =>
        {
            thirdPartyid = null;
            attorneyInvoiceMonthlyBreakdownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            metadata = new ServiceRequestMetadata();
            var OldThirdPartyInvoice = new ThirdPartyInvoiceDataModel("reference", 111210, 112, thirdPartyid, "FF0011", DateTime.Now, null, 100.00M, 14.00M, 114.00M,
                true, DateTime.Now, string.Empty);

            @event = new ThirdPartyInvoiceRejectedPreApprovalEvent(DateTime.Now, OldThirdPartyInvoice.AccountKey, null, OldThirdPartyInvoice.InvoiceNumber, null, "Rejected"
                , OldThirdPartyInvoice.SahlReference, OldThirdPartyInvoice.ThirdPartyInvoiceKey, "Subject", OldThirdPartyInvoice.ThirdPartyId);
            projector = new ThirdPartyInvoiceRejectedPreApprovalHandler(attorneyInvoiceMonthlyBreakdownManager);
        };

        private Because of = () =>
        {
            projector.Handle(@event, metadata);
        };

        private It should_not_decrement_the_unprocessed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.WasNotToldTo(x => x.DecrementUnProcessedCountForAttorney(Arg.Any<Guid>()));
        };
    }
}