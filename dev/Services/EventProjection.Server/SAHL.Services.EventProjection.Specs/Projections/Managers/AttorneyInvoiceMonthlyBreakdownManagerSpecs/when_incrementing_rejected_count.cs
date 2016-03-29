using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_incrementing_rejected_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager thirdPartyInvoiceDataManager;
        private static Guid thirdPartyId;

        private Establish that = () =>
        {
            thirdPartyId = Guid.NewGuid();
            thirdPartyInvoiceDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(thirdPartyInvoiceDataManager);
        };

        private Because of = () =>
            {
                attorneyInvoiceMonthlyBreakdownManager.IncrementRejectedCountForAttorney(thirdPartyId);
            };

        private It should_use_the_data_manager_to_increment_the_unprocessed_count = () =>
        {
            thirdPartyInvoiceDataManager.Received().IncrementRejectedCount(thirdPartyId);
        };
    }
}