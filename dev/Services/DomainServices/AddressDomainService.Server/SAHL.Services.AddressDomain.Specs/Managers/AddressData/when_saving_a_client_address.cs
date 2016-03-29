using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_saving_a_client_address : WithFakes
    {
        private static AddressDataManager addressDataService;
        private static FakeDbFactory dbFactory;
        private static ClientAddressModel clientAddress;
        private static int clientKey, addressKey, legalEntityAddressKey, result;
        private static AddressType addressType;

        private Establish context = () =>
        {
            legalEntityAddressKey = 4833;
            clientKey = 11;
            addressKey = 33;
            addressType = AddressType.Postal;
            clientAddress = new ClientAddressModel(clientKey, addressKey, addressType);
            dbFactory = new FakeDbFactory();
            addressDataService = new AddressDataManager(dbFactory);

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert(Param.IsAny<LegalEntityAddressDataModel>()))
                .Callback<LegalEntityAddressDataModel>(y =>
                {
                    y.LegalEntityAddressKey = legalEntityAddressKey;
                });
        };

        private Because of = () =>
        {
            result = addressDataService.SaveClientAddress(clientAddress);
        };

        private It should_insert_a_client_address_with_the_params_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<LegalEntityAddressDataModel>(y => y.AddressKey == clientAddress.AddressKey)));
        };

        private It should_return_a_system_generated_client_address_key = () =>
        {
            result.ShouldEqual(legalEntityAddressKey);
        };
    }
}
