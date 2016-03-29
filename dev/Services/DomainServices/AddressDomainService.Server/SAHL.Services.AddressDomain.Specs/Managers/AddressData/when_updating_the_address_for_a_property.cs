using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Managers.Statements;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    internal class when_updating_the_address_for_a_property : WithFakes
    {
        private static AddressDataManager addressDataService;
        private static FakeDbFactory dbFactory;
        private static int propertyKey;
        private static int addressKey;

        private Establish context = () =>
            {
                propertyKey = 13;
                addressKey = 7;
                dbFactory = new FakeDbFactory();
                addressDataService = new AddressDataManager(dbFactory);
            };

        private Because of = () =>
            {
                addressDataService.LinkAddressToProperty(propertyKey, addressKey);
            };

        private It should_link_the_address_to_the_property_with_the_params_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<PropertyDataModel>(Arg.Is<SetPropertyAddressStatement>(y => y.AddressKey == addressKey && y.PropertyKey == propertyKey)));
        };
    }
}