using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownManagerSpecs
{
    public class when_ensuring_the_projection_record_exists : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakdownManager;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakdownDataManager;
        private static Guid thirdPartyId;
        private static string registeredName;

        private Establish that = () =>
        {
            registeredName = "Attorney Registered Name";
            thirdPartyId = Guid.NewGuid();
            attorneyInvoiceMonthlyBreakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            attorneyInvoiceMonthlyBreakdownDataManager.WhenToldTo(x => x.GetRegisteredNameForAttorney(thirdPartyId)).Return(registeredName);
            attorneyInvoiceMonthlyBreakdownManager = new AttorneyInvoiceMonthlyBreakdownManager(attorneyInvoiceMonthlyBreakdownDataManager);
        };

        private Because of = () =>
        {
            attorneyInvoiceMonthlyBreakdownManager.EnsureProjectionRecordIsCreatedForAttorney(thirdPartyId);
        };

        private It should_get_the_registered_name_of_the_attorney = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.WasToldTo(x => x.GetRegisteredNameForAttorney(thirdPartyId));
        };

        private It should_ensure_an_initial_projection_record_exists = () =>
        {
            attorneyInvoiceMonthlyBreakdownDataManager.Received().MergeAttorneyMonthlyBreakdownRecordForAttorney(thirdPartyId, registeredName);
        };
    }
}