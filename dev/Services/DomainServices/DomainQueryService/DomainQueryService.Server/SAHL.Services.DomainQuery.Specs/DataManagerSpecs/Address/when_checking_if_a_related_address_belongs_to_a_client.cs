using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DomainQuery.Managers.Address;
using SAHL.Services.DomainQuery.Managers.Address.Statements;

namespace SAHL.Services.DomainQuery.Specs.DataManagerSpecs.Address
{
    public class when_checking_if_a_related_address_belongs_to_a_client : WithFakes
    {
        private static AddressDataManager addressDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static bool result;
        private static int addressKey;
        private static int clientKey;

        private Establish context = () =>
        {
            addressKey = 111;
            clientKey = 222;
            fakeDbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(
                x => x.SelectOne(Param.IsAny<AddressIsAClientAddressStatement>())).Return(1);
        };

        private Because of = () =>
        {
            result = addressDataManager.IsAddressAClientAddress(addressKey, clientKey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_use_the_address_key_and_client_key_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x=>x.SelectOne(Arg.Is<AddressIsAClientAddressStatement>(
                y=>y.AddressKey == addressKey && y.ClientKey == clientKey)));
        };
    }
}