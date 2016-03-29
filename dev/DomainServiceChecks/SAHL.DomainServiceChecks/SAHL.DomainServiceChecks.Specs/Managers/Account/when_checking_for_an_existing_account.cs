using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;
using SAHL.DomainServiceChecks.Managers.AccountDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Account
{
    public class when_checking_for_an_existing_account : WithCoreFakes
    {
        private static IAccountDataManager accountDataManager;
        private static int accountKey;
        private static bool accountExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            accountKey = 1;
            accountDataManager = new AccountDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<AccountExistsStatement>())).Return(1);
        };

        private Because of = () =>
        {
            accountExistsResponse = accountDataManager.DoesAccountExist(accountKey);
        };

        private It should_check_if_the_property_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<AccountExistsStatement>(y => y.AccountKey == accountKey)));
        };

        private It should_return_that_the_account_exsits = () =>
        {
            accountExistsResponse.ShouldBeTrue();
        };
    }
}