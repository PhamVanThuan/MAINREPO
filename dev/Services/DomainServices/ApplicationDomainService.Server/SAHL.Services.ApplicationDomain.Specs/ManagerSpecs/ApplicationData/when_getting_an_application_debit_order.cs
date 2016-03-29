using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_an_application_debit_order : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static ApplicationDataManager manager;
        private static int applicationNumber;
        private static IEnumerable<OfferDebitOrderDataModel> applicationDebitOrders;

        private Establish context = () =>
            {
                applicationNumber = 1234;
                dbFactory = new FakeDbFactory();
                manager = new ApplicationDataManager(dbFactory);
            };

        private Because of = () =>
            {
                applicationDebitOrders = manager.GetApplicationDebitOrder(applicationNumber);
            };

        private It should = () =>
            {
                dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<OfferDebitOrderDataModel>(Param.IsAny<GetApplicationDebitOrderStatement>()));
            };
    }
}