using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_latest_offerinformation : WithCoreFakes
    {
        private static ApplicationDataManager _financialDataManager;
        private static int _applicationNumber;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            _financialDataManager = new ApplicationDataManager(dbFactory);
            _applicationNumber    = 1;
        };

        private Because of = () =>
        {
            _financialDataManager.GetLatestApplicationOfferInformation(_applicationNumber);
        };

        private It should_query_for_the_latest_offer_information_type = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<OfferInformationDataModel>(Arg.Is<GetLatestApplicationOfferInformationStatement>(
                y => y.ApplicationNumber == _applicationNumber)));
        };
    }
}
