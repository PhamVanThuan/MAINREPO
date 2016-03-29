using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Managers.Statements;
using System;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_getting_an_existing_active_client_address : WithFakes
    {
        private static AddressDataManager addressDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static LegalEntityAddressDataModel legalEntityAddress;
        private static LegalEntityAddressDataModel result;
        private static int clientKey;
        private static int addressKey;
        private static AddressType addressType;

        private Establish context = () =>
        {
            clientKey = 1;
            addressKey = 2;
            addressType = AddressType.Postal;
            legalEntityAddress = new LegalEntityAddressDataModel(1, 2, 1, DateTime.Now, 1);
            fakeDbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<LegalEntityAddressDataModel>(Param.IsAny<ISqlStatement<LegalEntityAddressDataModel>>()))
                .Return(legalEntityAddress);
        };

        private Because of = () =>
        {
            result = addressDataManager.GetExistingActiveClientAddress(clientKey, addressKey, addressType);
        };

        private It should_use_the_client_and_address_provided_when_querying_the_database = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<LegalEntityAddressDataModel>(Arg.Is<GetExistingActiveClientAddressStatement>(
                y=>y.ClientKey == clientKey && y.AddressKey == addressKey)));
        };

        private It should_only_look_for_active_client_addresses = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<LegalEntityAddressDataModel>(Arg.Is<GetExistingActiveClientAddressStatement>(
                y=>y.GeneralStatusKey == (int)GeneralStatus.Active
                )));
        };

        private It should_include_the_address_type_of_the_client_address = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<LegalEntityAddressDataModel>(Arg.Is<GetExistingActiveClientAddressStatement>(
                y => y.AddressType == (int)addressType
                )));
        };
    }
}