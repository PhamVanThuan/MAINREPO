using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.Managers.ThirdPartyInvoice
{
    public class when_getting_a_third_party_invoice_and_one_exists : WithFakes
    {
        private static ThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static int thirdPartyInvoiceKey;
        private static int countFromDatabaseQuery;
        private static bool result;

        private Establish context = () =>
        {
            countFromDatabaseQuery = 1;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(fakeDbFactory);
            thirdPartyInvoiceKey = 1;
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Arg.Any<ThirdPartyInvoiceExistsStatement>()))
                .Return(countFromDatabaseQuery);
        };

        private Because of = () =>
        {
            result = dataManager.DoesThirdPartyInvoiceExist(thirdPartyInvoiceKey);
        };

        private It should_check_for_the_invoice_using_the_correct_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x =>
                x.SelectOne(Arg.Is<ThirdPartyInvoiceExistsStatement>(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}