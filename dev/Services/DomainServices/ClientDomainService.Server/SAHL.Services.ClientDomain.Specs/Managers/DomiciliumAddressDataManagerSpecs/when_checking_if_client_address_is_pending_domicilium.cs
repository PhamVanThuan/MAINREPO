﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_checking_if_client_address_is_pending_domicilium : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static ClientAddressIsPendingDomiciliumStatement query;
        private static int clientAddressKey;
        private static bool result;
        static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            clientAddressKey = 12345;
            dbFactory = new FakeDbFactory();
            domiciliumAddressManager = new DomiciliumAddressDataManager(dbFactory);
            query = new ClientAddressIsPendingDomiciliumStatement(clientAddressKey);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<ClientAddressIsPendingDomiciliumStatement>()))
                .Return(1);
        };

        private Because of = () =>
        {
            result = domiciliumAddressManager.IsClientAddressPendingDomicilium(clientAddressKey);
        };

        private It should_check_for_client_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<ClientAddressIsPendingDomiciliumStatement>()));
        };

        private It should_find_client_address = () =>
        {
            result.ShouldBeTrue();
        };
    }
}