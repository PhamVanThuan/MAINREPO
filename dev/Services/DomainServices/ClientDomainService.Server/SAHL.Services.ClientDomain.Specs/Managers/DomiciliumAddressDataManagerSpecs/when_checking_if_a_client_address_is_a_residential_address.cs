﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_checking_if_a_client_address_is_a_residential_address : WithFakes
    {

        static IDomiciliumAddressDataManager domicilumDataManager;
        static int clientAddressKey;
        static bool response;
        static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            clientAddressKey = 100;
            domicilumDataManager = new DomiciliumAddressDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<AddressTypeIsAResidentialAddressTypeStatement>())).Return(1);
        };

        Because of = () =>
        {
            response = domicilumDataManager.CheckIsAddressTypeAResidentialAddress(clientAddressKey);
        };

        It should_check_if_address_is_a_residential_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<AddressTypeIsAResidentialAddressTypeStatement>()));
        };

        It should_confirm_addresss_is_residential_address = () =>
        {
            response.ShouldBeTrue();
        };
    }
}