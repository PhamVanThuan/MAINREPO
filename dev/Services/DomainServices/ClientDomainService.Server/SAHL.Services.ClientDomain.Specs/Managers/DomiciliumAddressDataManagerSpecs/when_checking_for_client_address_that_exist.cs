using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_checking_for_client_address_that_exist : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static IEnumerable<LegalEntityAddressDataModel> existingClientAddress;
        private static IEnumerable<LegalEntityAddressDataModel> result;
        private static int clientAddressKey;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            clientAddressKey = 12345;
            existingClientAddress = new List<LegalEntityAddressDataModel> { new LegalEntityAddressDataModel(clientAddressKey, 322, 1, DateTime.Now, (int)GeneralStatus.Pending) };
            domiciliumAddressManager = new DomiciliumAddressDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<LegalEntityAddressDataModel>(Param.IsAny<ISqlStatement<LegalEntityAddressDataModel>>()))
                .Return(existingClientAddress);
        };

        private Because of = () =>
        {
            result = domiciliumAddressManager.FindExistingActiveClientAddress(clientAddressKey);
        };

        private It should_check_for_client_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<LegalEntityAddressDataModel>(Param.IsAny<ISqlStatement<LegalEntityAddressDataModel>>()));
        };

        private It should_find_client_address = () =>
        {
            result.Count().ShouldBeGreaterThan(0);
        };
    }
}