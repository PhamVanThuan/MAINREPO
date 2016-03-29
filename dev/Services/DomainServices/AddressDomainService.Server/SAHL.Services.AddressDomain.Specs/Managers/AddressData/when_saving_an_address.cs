using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Testing.Providers;
using SAHL.Services.AddressDomain.Managers;
using System;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_saving_an_address : WithFakes
    {
        private static ILinkedKeyManager linkedKeyManager;
        private static AddressDataManager addressDataService;
        private static FakeDbFactory dbFactory;
        private static AddressDataModel newAddress;
        private static int addressFormatKey, addressKey, expectedAddressKey;
        private static Guid addressGuid;

        private Establish context = () =>
        {
            expectedAddressKey = 4688;
            addressFormatKey = 1;
            newAddress = new AddressDataModel(addressFormatKey, null, "", "5", "Camberwell", "21", "Somerset Drive", 
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            addressGuid = CombGuid.Instance.Generate();
            linkedKeyManager = An<ILinkedKeyManager>();
            dbFactory = new FakeDbFactory();
            addressDataService = new AddressDataManager(dbFactory);
            
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<AddressDataModel>(newAddress))
                .Callback<AddressDataModel>(y => { y.AddressKey = expectedAddressKey; });
        };

        private Because of = () =>
        {
            addressKey = addressDataService.SaveAddress(newAddress);
        };

        private It should_insert_an_address_with_the_params_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<AddressDataModel>(Arg.Is<AddressDataModel>(y => y.AddressFormatKey == addressFormatKey)));
        };

        private It should_return_a_system_generated_address_key = () =>
        {
            addressKey.ShouldEqual(expectedAddressKey);
        };
    }
}