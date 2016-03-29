using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_finding_client_pending_domicilium_address_that_does_not_exist: WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static IEnumerable<LegalEntityDomiciliumDataModel> existingClientDomicilium;
        private static IEnumerable<LegalEntityDomiciliumDataModel> result;
        private static int clientAddressKey;
        static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            domiciliumAddressManager = new DomiciliumAddressDataManager(dbFactory);
            existingClientDomicilium = new List<LegalEntityDomiciliumDataModel>();
            clientAddressKey = 1232;
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<LegalEntityDomiciliumDataModel>(Param.IsAny<ISqlStatement<LegalEntityDomiciliumDataModel>>()))
                .Return(existingClientDomicilium);
        };

        private Because of = () =>
        {
            result = domiciliumAddressManager.FindExistingClientPendingDomicilium(clientAddressKey);
        };

        private It should_query_for_existing_client_domicilium_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<LegalEntityDomiciliumDataModel>(Param.IsAny<ISqlStatement<LegalEntityDomiciliumDataModel>>()));
        };

        private It should_not_return_any_records = () =>
        {
            result.Count().ShouldEqual(0);
        };

    }
}
