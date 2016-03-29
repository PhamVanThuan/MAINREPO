using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    internal class when_getting_the_attorney_registered_name : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;
        private static string registeredName;
        private static string result;

        private Establish context = () =>
        {
            thirdPartyId = Guid.NewGuid();
            registeredName = "Randles (Inc)";
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetAttorneyRegisteredNameStatement>())).Return(registeredName);
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);            
        };

        private Because of = () =>
        {
            result = dataManager.GetRegisteredNameForAttorney(thirdPartyId);
        };

        private It should_use_the_correct_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetAttorneyRegisteredNameStatement>
                .Matches(y => y.AttorneyId == thirdPartyId)));
        };

        private It should_return_the_result_of_the_statement = () =>
        {
            result.ShouldEqual(registeredName);
        };
    }
}