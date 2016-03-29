using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_checking_for_client_address_that_does_not_exist : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static IEnumerable<LegalEntityAddressDataModel> existingClientAddress;
        private static IEnumerable<LegalEntityAddressDataModel> result;
        private static int clientAddressKey;
        static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            existingClientAddress = new List<LegalEntityAddressDataModel>();
            clientAddressKey = 12345;
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

        private It should_not_find_any_record = () =>
        {
            result.Count().ShouldEqual(0);
        };
    }
}
