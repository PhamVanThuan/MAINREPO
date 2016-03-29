using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;
using NSubstitute;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_decrementing_processed_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static Guid thirdParty;

        private Establish that = () =>
        {
            thirdParty = Guid.NewGuid();
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
            {
                attorneyInvoiceMonthlyBreakdownManager.DecrementProcessedCountForAttorney(thirdParty);
            };

        private It Should_adjust_processed_for_the_attorney = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.Received().AdjustProcessedCount(thirdParty, -1);
        };

    }
}