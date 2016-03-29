using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_saving_client_domicilium_address: WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static LegalEntityDomiciliumDataModel clientAddressAsPendingDomicilium;

        private static int result;
        private static int userKey;
        private static int clientDomiciliumKey;
        static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            userKey = 121;
            dbFactory = new FakeDbFactory();
            domiciliumAddressManager = new DomiciliumAddressDataManager(dbFactory);
            clientDomiciliumKey = 232;
            clientAddressAsPendingDomicilium = new LegalEntityDomiciliumDataModel(121, (int)GeneralStatus.Pending, DateTime.Now, userKey);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<LegalEntityDomiciliumDataModel>(Param.IsAny<LegalEntityDomiciliumDataModel>()))
                .Callback<LegalEntityDomiciliumDataModel>(y => { y.LegalEntityDomiciliumKey = clientDomiciliumKey; });

        };

        private Because of = () =>
        {
            result = domiciliumAddressManager.SavePendingDomiciliumAddress(clientAddressAsPendingDomicilium);
        };

        private It should_save_record_to_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<LegalEntityDomiciliumDataModel>(clientAddressAsPendingDomicilium));
        };

        private It should_return_generated_domicilium_key = () =>
        {
            result.ShouldEqual(clientDomiciliumKey);
        };

    }
}
