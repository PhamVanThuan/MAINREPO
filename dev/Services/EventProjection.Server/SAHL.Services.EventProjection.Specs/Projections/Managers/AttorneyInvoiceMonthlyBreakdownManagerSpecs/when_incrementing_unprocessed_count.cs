using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_incrementing_unprocessed_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static Guid thirdPartyId;

        private Establish that = () =>
        {
            thirdPartyId = Guid.NewGuid();
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
            {
                attorneyInvoiceMonthlyBreakdownManager.IncrementUnProcessedCountForAttorney(thirdPartyId);
            };

        private It Should_adjust_unprocessed_for_the_attorny = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.Received().AdjustUnprocessedCount(thirdPartyId, +1);
        };
    }
}