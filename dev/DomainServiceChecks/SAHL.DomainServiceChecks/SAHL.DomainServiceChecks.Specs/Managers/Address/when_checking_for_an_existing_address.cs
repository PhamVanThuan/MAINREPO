using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.AddressDataManager;
using SAHL.DomainServiceChecks.Managers.AddressDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Address
{
    public class when_checking_for_an_existing_address : WithFakes
    {
        private static IAddressDataManager addressDataManager;
        private static int addressKey;
        private static bool addressExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            addressKey = 1;
            addressDataManager = new AddressDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<AddressExistsStatement>())).Return(1);
        };

        private Because of = () =>
        {
            addressExistsResponse = addressDataManager.IsAddressInOurSystem(addressKey);
        };

        private It should_check_if_the_property_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<AddressExistsStatement>(y => y.AddressKey == addressKey)));
        };

        private It should_acknolewdge_that_the_property_exists_in_our_system = () =>
        {
            addressExistsResponse.ShouldBeTrue();
        };
    }
}