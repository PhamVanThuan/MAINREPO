using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_getting_the_third_party_id : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;
        private static int thirdPartyInvoiceKey;
        private static Guid result;

        private Establish context = () =>
        {
            thirdPartyInvoiceKey = 1344;
            thirdPartyId = Guid.NewGuid();
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetThirdPartyIdForInvoiceStatement>())).Return(thirdPartyId);
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetThirdPartyIdByThirdPartyInvoiceKey(thirdPartyInvoiceKey);
        };

        private It should_use_the_correct_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetThirdPartyIdForInvoiceStatement>
                .Matches(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };

        private It should_return_the_result_of_the_statement = () =>
        {
            result.ShouldEqual(thirdPartyId);
        };
    }
}