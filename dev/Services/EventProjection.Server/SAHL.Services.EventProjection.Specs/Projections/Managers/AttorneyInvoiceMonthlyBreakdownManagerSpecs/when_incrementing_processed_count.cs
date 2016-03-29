using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_incrementing_processed_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static Guid thirdPartyId;
        private static int valueToAdd;

        private Establish that = () =>
        {
            thirdPartyId = Guid.NewGuid();
            valueToAdd = +1;
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.IncrementProcessedCountForAttorney(thirdPartyId);
        };

        private It should_use_the_data_manager_to_increment_the_processed_count = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.AdjustProcessedCount(thirdPartyId, valueToAdd);
        };
    }
}